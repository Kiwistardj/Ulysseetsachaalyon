"""Découpe le sol du ch1 en tuiles et en sprites de bâtiments.
Usage : python3 -I tools/export-ch1/cut-layers.py <dossier_export> <dossier_assets> <fichier_json>
Coordonnées en unités du jeu HTML (y vers le bas), image à 2 px par unité.
"""
import json, os, sys
from PIL import Image, ImageDraw

SRC, DST, JSON_OUT = sys.argv[1], sys.argv[2], sys.argv[3]
K = 2          # pixels par unité HTML
TILE = 2048    # taille max des tuiles (compatible téléphone)
PAD = 2        # marge pour garder le contour d'encre

# Formes de chaque bâtiment : rectangles arrondis (x, y, w, h, r) et ellipses ('e', cx, cy, rx, ry).
# Relevées dans buildBG() de ch1-perraliere-v2.html. 'base' = y du pied de la façade (tri de profondeur).
import math
OCT = [(790 + math.cos(math.pi / 8 + i * math.pi / 4) * 86, 430 + math.sin(math.pi / 8 + i * math.pi / 4) * 36) for i in range(8)]

BUILDINGS = {
    'immeuble_chez_nous': dict(base=1752, shapes=[(510, 1384, 500, 74, 14), (520, 1446, 480, 306, 8)]),
    'commerces':          dict(base=1240, shapes=[(550, 1026, 480, 70, 14), (558, 1084, 464, 158, 8)]),
    'immeuble_nanou':     dict(base=1730, shapes=[(1290, 1464, 360, 70, 14), (1300, 1526, 340, 204, 8)]),
    'local_velos':        dict(base=1696, shapes=[(1654, 1632, 120, 16, 5), (1660, 1640, 108, 56, 6)]),
    'ecole':              dict(base=1558, shapes=[(1828, 1284, 824, 70, 14), (1840, 1344, 800, 214, 8)]),
    'immeuble_elisa':     dict(base=2160, shapes=[(40, 1952, 300, 56, 12), (50, 2000, 280, 160, 8)]),
    'bouche_metro':       dict(base=226,  shapes=[(718, 80, 140, 36, 10), (692, 128, 192, 30, 10), (700, 150, 176, 76, 8), (784, 112, 8, 20, 0)]),
    'kiosque':            dict(base=524,  shapes=[('poly', [(706, 436), (790, 382), (874, 436)]), ('poly', OCT), ('poly', [(732, 440), (848, 440), (848, 496), (732, 496)]), ('e', 790, 494, 72, 30)]),
    'bateau':             dict(base=604,  shapes=[(316, 420, 8, 112, 3), ('poly', [(326, 428), (392, 500), (326, 500)]), ('poly', [(324, 420), (350, 428), (324, 436)]), (236, 528, 170, 16, 5), ('poly', [(226, 540), (416, 540), (404, 600), (370, 604), (268, 604), (236, 600)])]),
}

def mask_for(shapes, size, ox, oy):
    m = Image.new('L', size, 0)
    d = ImageDraw.Draw(m)
    for s in shapes:
        if s[0] == 'e':
            _, cx, cy, rx, ry = s
            d.ellipse([((cx - rx - PAD) - ox) * K, ((cy - ry - PAD) - oy) * K, ((cx + rx + PAD) - ox) * K, ((cy + ry + PAD) - oy) * K], fill=255)
        elif s[0] == 'poly':
            d.polygon([((x - ox) * K, (y - oy) * K) for x, y in s[1]], fill=255)
        else:
            x, y, w, h, r = s
            d.rounded_rectangle([((x - PAD) - ox) * K, ((y - PAD) - oy) * K, ((x + w + PAD) - ox) * K, ((y + h + PAD) - oy) * K], radius=(r + PAD) * K, fill=255)
    return m

def bbox(shapes):
    xs, ys = [], []
    for s in shapes:
        if s[0] == 'e':
            _, cx, cy, rx, ry = s; xs += [cx - rx, cx + rx]; ys += [cy - ry, cy + ry]
        elif s[0] == 'poly':
            xs += [p[0] for p in s[1]]; ys += [p[1] for p in s[1]]
        else:
            x, y, w, h, _ = s; xs += [x, x + w]; ys += [y, y + h]
    return min(xs) - PAD - 1, min(ys) - PAD - 1, max(xs) + PAD + 1, max(ys) + PAD + 1

img = Image.open(os.path.join(SRC, 'sol_complet.png')).convert('RGBA')
W, H = img.size
os.makedirs(os.path.join(DST, 'sol'), exist_ok=True)
os.makedirs(os.path.join(DST, 'batiments'), exist_ok=True)

tiles = []
for ty in range(0, H, TILE):
    for tx in range(0, W, TILE):
        t = img.crop((tx, ty, min(tx + TILE, W), min(ty + TILE, H)))
        name = f'sol_{tx // TILE}_{ty // TILE}'
        t.convert('RGB').save(os.path.join(DST, 'sol', name + '.jpg'), quality=92)
        # coin haut gauche de la tuile, en unités HTML
        tiles.append(dict(name=name, x=tx / K, y=ty / K, w=t.size[0] / K, h=t.size[1] / K))

blds = []
for name, b in BUILDINGS.items():
    x0, y0, x1, y1 = bbox(b['shapes'])
    crop = img.crop((int(x0 * K), int(y0 * K), int(x1 * K), int(y1 * K)))
    m = mask_for(b['shapes'], crop.size, int(x0 * K) / K, int(y0 * K) / K)
    crop.putalpha(m)
    crop.save(os.path.join(DST, 'batiments', name + '.png'), optimize=True)
    blds.append(dict(name=name, x=int(x0 * K) / K, y=int(y0 * K) / K, w=crop.size[0] / K, h=crop.size[1] / K, base=b['base']))

data = json.load(open(os.path.join(SRC, 'layout_raw.json')))
data['tiles'] = tiles
data['buildings'] = blds
data['pxPerUnit'] = K
json.dump(data, open(JSON_OUT, 'w'), ensure_ascii=False, indent=1)
print(len(tiles), 'tuiles,', len(blds), 'bâtiments')
