// Export du décor du ch1 (v2) en calques pour Unity.
// - sol en tuiles 2048 px (2 px par unité du jeu HTML)
// - un sprite par bâtiment (découpé dans le sol), pour la profondeur
// - toutes les positions du code (collisions, arbres, PNJ, quêtes) en JSON
// Usage : node tools/export-ch1/export-layers.js <sortie_tmp>
const { chromium } = require('playwright');
const fs = require('fs');
const path = require('path');

const ROOT = path.resolve(__dirname, '../..');
const SRC = path.join(ROOT, 'reference-html/ch1-perraliere-v2.html');
const OUT = process.argv[2] || path.join(ROOT, 'Temp/export-ch1');
const SCALE = 2;

const EXPO = 'window.__E={buildBG,get BG(){return BG},RECTS,POSTS,PEDS,ARAU,SHOPS,TREES,CARS,LANES,METRO,RONCES,BUSH,ITEMS,HURT,NPCS:NPCS.map(n=>({id:n.id,x:n.x,y:n.y,dir:n.dir,first:n.first||null,again:typeof n.again==="string"?n.again:null})),LAMPS,MAILBOX,BFLY,P:{x:P.x,y:P.y},F:Object.fromEntries(Object.entries(F).map(([k,v])=>[k,{x:v.x,y:v.y,idx:v.idx}])),RAMPS,CH_WP,CHEER,WW,WH};';

(async () => {
  fs.mkdirSync(OUT, { recursive: true });
  let src = fs.readFileSync(SRC, 'utf8');
  if (!src.includes('let last=performance.now()')) throw new Error('point d\'injection introuvable');
  src = src.replace('let last=performance.now()', EXPO + 'let last=performance.now()');
  src = src.replace("cnv.width=WW;cnv.height=WH;const g=cnv.getContext('2d');",
    `cnv.width=WW*${SCALE};cnv.height=WH*${SCALE};const g=cnv.getContext('2d');g.scale(${SCALE},${SCALE});`);
  // règle du projet : pas de nom de marque dessiné
  src = src.replace("'#ffffff','Casino'", "'#ffffff','Épicerie'");
  const page = path.join(OUT, 'page.html');
  fs.writeFileSync(page, src);

  const b = await chromium.launch();
  const pg = await b.newPage({ viewport: { width: 1280, height: 720 } });
  const errs = [];
  pg.on('pageerror', e => errs.push(String(e).slice(0, 200)));
  await pg.goto('file://' + page);
  await pg.evaluate(() => document.fonts.ready);
  await pg.waitForTimeout(1500);
  // redessine une fois les polices chargées (les panneaux utilisent Grandstander et Andika)
  const fontsOk = await pg.evaluate(() => document.fonts.check('800 13px Grandstander') && document.fonts.check('700 11px Andika'));
  await pg.evaluate(() => window.__E.buildBG());
  const bg = await pg.evaluate(() => window.__E.BG.toDataURL('image/png'));
  fs.writeFileSync(path.join(OUT, 'sol_complet.png'), Buffer.from(bg.split(',')[1], 'base64'));
  const data = await pg.evaluate(() => JSON.parse(JSON.stringify(window.__E, (k, v) => typeof v === 'function' ? undefined : v)));
  delete data.BG;
  fs.writeFileSync(path.join(OUT, 'layout_raw.json'), JSON.stringify(data, null, 1));
  console.log(JSON.stringify({ fontsOk, errs, size: [data.WW * SCALE, data.WH * SCALE] }));
  await b.close();
})();
