import asyncio, os, base64, json, sys
from playwright.async_api import async_playwright
HERE = os.path.dirname(os.path.abspath(__file__))
OUT = os.path.join(HERE, 'out')
EXPO = ("window.E={S,LOOK,BUSH,ITEMS,drawPerson,drawCat,drawFox,drawPanther,drawRex,drawHedgehog,drawRobin,drawPigeon,"
        "drawButterfly,drawTree,drawLamp,drawMailbox,drawPedLight,drawAraucaria,drawFountain,drawRonce,drawBush,drawRamp,"
        "drawScooterParked,drawCar,drawPortrait,get BG(){return BG}};")
# 4 pixels par unité du jeu ; k = échelle utilisée dans le jeu pour cet élément
U = 4
P = "{noShadow:true}"
def person(i, opt=P, d='down'): return f"c=>E.drawPerson(c,0,0,E.LOOK.{i},'{d}',0,false,{opt})"
ITEMS = []
def add(folder, name, code, k=1.0, kd=None, t=0.5, pre=''):
    # kd : échelle passée à la fonction (>=1.6 coupe l'ombre des animaux) ; l'échelle du canvas compense
    ITEMS.append(dict(folder=folder, name=name, code=code, scale=U * k / (kd or k) if kd else U * k, t=t, pre=pre))
humans = ['ulysse','sacha','papa','maman','nanou','elisa','lucia','leonie','victorine','helena','maya','iris','prune','mahe']
for h in humans: add('personnages', h, person(h))
add('personnages', 'ulysse_trottinette', person('ulysse', "{noShadow:true,scooter:'#3fa34d',helmet:'#f0862a'}"))
add('personnages', 'ulysse_trottinette_profil', person('ulysse', "{noShadow:true,scooter:'#3fa34d',helmet:'#f0862a'}", 'right'))
add('personnages', 'sacha_trottinette', person('sacha', "{noShadow:true,scooter:'#22407a',helmet:'#9fd4f2'}"))
add('personnages', 'sacha_trottinette_profil', person('sacha', "{noShadow:true,scooter:'#22407a',helmet:'#9fd4f2'}", 'right'))
add('animaux', 'bibou', "c=>E.drawCat(c,0,0,1,0,false,2)", k=1, kd=2)
add('animaux', 'didou', "c=>E.drawFox(c,0,0,1,0,false,2)", k=1.05, kd=2)
add('animaux', 'ninou', "c=>E.drawPanther(c,0,0,1,0,false,2,false)", k=.95, kd=2)
add('animaux', 'sacha_sur_ninou', "c=>E.drawPanther(c,0,0,1,0,false,2,true)", k=.95, kd=2)
add('animaux', 'rex', "c=>E.drawRex(c,0,0,1,0,false,2)", k=1.2, kd=2)
add('animaux', 'herisson_blesse', "c=>E.drawHedgehog(c,0,0,1,true,2)", k=1.15, kd=2)
add('animaux', 'herisson_gueri', "c=>E.drawHedgehog(c,0,0,1,false,2)", k=1.15, kd=2)
add('animaux', 'rougegorge_blesse', "c=>E.drawRobin(c,0,0,1,true,2,false)", k=1.35, kd=2)
add('animaux', 'rougegorge_gueri', "c=>E.drawRobin(c,0,0,1,false,2,false)", k=1.35, kd=2)
add('animaux', 'rougegorge_vol', "c=>E.drawRobin(c,0,0,1,false,2,true)", k=1.35, kd=2, t=0.05)
add('animaux', 'pigeon', "c=>E.drawPigeon(c,{x:0,y:0,face:1,peck:0,fly:1})", t=0)
for col, n in [('#ffd25a','jaune'),('#ff9ec0','rose'),('#9fd0f5','bleu'),('#ffffff','blanc')]:
    add('animaux', 'papillon_' + n, "c=>E.drawButterfly(c,{cx:0,cy:0,ph:Math.PI/2,col:'%s'})" % col, t=0)
for col, n in [('#3fa34d','verte_ulysse'),('#22407a','bleue_sacha'),('#a679d8','violette_iris'),('#f08a4b','orange_victorine'),('#e4567b','rose_leonie')]:
    add('vehicules', 'trottinette_' + n, "c=>E.drawScooterParked(c,0,0,'%s')" % col)
for col, n, fire in [('#4f8fd8','bleue',0),('#f2b440','jaune',0),('#6cc36a','verte',0),('#e04a3a','pompiers',1)]:
    add('vehicules', 'voiture_' + n, "c=>E.drawCar(c,{x:0,y:0,w:110,h:56,color:'%s',fire:%s,bounce:0,siren:0})" % (col, 'true' if fire else 'false'))
add('objets', 'arbre', "c=>E.drawTree(c,{x:0,y:0,seed:0,bloom:false})", t=0)
add('objets', 'arbre_fleuri', "c=>E.drawTree(c,{x:0,y:0,seed:0,bloom:true})", t=0)
add('objets', 'araucaria', "c=>E.drawAraucaria(c,{x:0,y:0})", t=0)
add('objets', 'lampadaire', "c=>E.drawLamp(c,{x:0,y:0})")
add('objets', 'boite_aux_lettres', "c=>E.drawMailbox(c,{x:0,y:0})")
add('objets', 'feu_pieton_rouge', "c=>{E.S.ped='red';E.drawPedLight(c,{x:0,y:0})}")
add('objets', 'feu_pieton_vert', "c=>{E.S.ped='green';E.drawPedLight(c,{x:0,y:0})}")
add('objets', 'fontaine_jets', "c=>{c.translate(-1060,-330);E.drawFountain(c)}", t=0.3)
add('objets', 'ronce', "c=>E.drawRonce(c,{x:0,y:0,r:32,burning:0,gone:false})")
add('objets', 'ronce_qui_brule', "c=>E.drawRonce(c,{x:0,y:0,r:32,burning:.45,gone:false})")
add('objets', 'fente_local_velos_avec_trottinette', "c=>{E.ITEMS[1].found=false;E.BUSH.rustle=0;c.translate(-1714,-1694);E.drawBush(c)}")
add('objets', 'fente_local_velos_vide', "c=>{E.ITEMS[1].found=true;E.BUSH.rustle=0;c.translate(-1714,-1694);E.drawBush(c)}")
add('objets', 'tremplin', "c=>E.drawRamp(c,{x:0,y:0})")

PAGE_JS = r"""
(async (it)=>{
  E.S.t=it.t;
  const W=2600,H=2600,cv=document.createElement('canvas');cv.width=W;cv.height=H;const c=cv.getContext('2d');
  c.setTransform(it.scale,0,0,it.scale,W/2,H*0.7);
  (eval(it.code))(c);
  c.setTransform(1,0,0,1,0,0);
  const d=c.getImageData(0,0,W,H).data;let x0=W,y0=H,x1=-1,y1=-1;
  for(let y=0;y<H;y++)for(let x=0;x<W;x++){if(d[(y*W+x)*4+3]>2){if(x<x0)x0=x;if(x>x1)x1=x;if(y<y0)y0=y;if(y>y1)y1=y;}}
  if(x1<0)return {empty:true};
  const edge=x0<=0||y0<=0||x1>=W-1||y1>=H-1,pad=6,o=document.createElement('canvas');o.width=x1-x0+1+pad*2;o.height=y1-y0+1+pad*2;
  o.getContext('2d').drawImage(cv,x0-pad,y0-pad,o.width,o.height,0,0,o.width,o.height);
  return {url:o.toDataURL('image/png'),w:o.width,h:o.height,edge};
})
"""

async def main():
    os.makedirs(OUT, exist_ok=True)
    async with async_playwright() as p:
        b = await p.chromium.launch()
        pg = await (await b.new_context(viewport={'width': 1280, 'height': 720})).new_page()
        errs = []
        pg.on('pageerror', lambda e: errs.append(str(e)[:200]))
        src = open(os.path.join(HERE, 'ch1.html')).read().replace('let last=performance.now()', EXPO + 'let last=performance.now()').replace("cnv.width=WW;cnv.height=WH;const g=cnv.getContext('2d');","cnv.width=WW*2;cnv.height=WH*2;const g=cnv.getContext('2d');g.scale(2,2);")
        page = os.path.join(HERE, 'page.html')
        open(page, 'w').write('<!doctype html><html><head><meta charset="utf-8"></head><body>' + src + '</body></html>')
        await pg.goto('file://' + page); await pg.wait_for_timeout(3000)
        await pg.evaluate("window.speechSynthesis&&(speechSynthesis.speak=()=>{})")
        man = []
        for it in ITEMS:
            r = await pg.evaluate(PAGE_JS + "(%s)" % json.dumps(it))
            if r.get('empty'): print('VIDE', it['name']); continue
            d = os.path.join(OUT, it['folder']); os.makedirs(d, exist_ok=True)
            open(os.path.join(d, it['name'] + '.png'), 'wb').write(base64.b64decode(r['url'].split(',')[1]))
            man.append(dict(fichier=it['folder'] + '/' + it['name'] + '.png', largeur=r['w'], hauteur=r['h']))
            if r['edge']: print('BORD', it['name'])
        # portraits du dialogue (le carré de la bande de dialogue)
        ids = await pg.evaluate("Object.keys(E.LOOK)")
        os.makedirs(os.path.join(OUT, 'portraits'), exist_ok=True)
        await pg.evaluate('E.S.t=0.5')
        for i in ids:
            url = await pg.evaluate("(()=>{E.S.t=0.5;const c=document.createElement('canvas');c.width=c.height=384;E.drawPortrait(c,'%s');return c.toDataURL('image/png');})()" % i)
            open(os.path.join(OUT, 'portraits', i + '.png'), 'wb').write(base64.b64decode(url.split(',')[1]))
            man.append(dict(fichier='portraits/' + i + '.png', largeur=384, hauteur=384))
        # le décor de fond : la carte entière de la Perralière, et sa version x2
        url = await pg.evaluate("E.BG.toDataURL('image/png')")
        os.makedirs(os.path.join(OUT, 'decors'), exist_ok=True)
        open(os.path.join(OUT, 'decors', 'perraliere_carte_x2.png'), 'wb').write(base64.b64decode(url.split(',')[1]))
        man.append(dict(fichier='decors/perraliere_carte_x2.png', largeur=5440, hauteur=4480, note='carte entiere, 2 pixels par unite du jeu'))
        # la scène du métro (plein écran 1280 x 720, en pleine marche)
        await pg.evaluate("E.S.mode='metro';E.S.metroT=3.2;E.S.metroScroll=900;E.S.metroV=1600;document.querySelectorAll('.overlay,#dlg,.hud,.tools,#touch,.pad-chip').forEach(e=>e.hidden=true);")
        await pg.wait_for_timeout(600)
        os.makedirs(os.path.join(OUT, 'scenes'), exist_ok=True)
        await pg.screenshot(path=os.path.join(OUT, 'scenes', 'metro_ligne_A.png'))
        json.dump(man, open(os.path.join(OUT, 'manifeste.json'), 'w'), ensure_ascii=False, indent=1)
        print(len(man), 'images', 'erreurs:', errs)
        await b.close()
asyncio.run(main())
