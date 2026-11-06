"""Assemble les 9 chapitres dans une seule app (dossier app/).

Les chapitres sont copiés TELS QUELS depuis reference-html/. Seul ajout : un petit relais
en fin de page qui
  - garde dans l'app les liens vers les autres chapitres (au lieu d'ouvrir un autre onglet),
  - prévient l'app quand l'écran de fin du chapitre s'affiche (sauvegarde, chapitre suivant).
Aucune réplique, scène ni règle de jeu n'est modifiée.

Usage : python3 -I tools/assemble/build.py
"""
import json, os, shutil

ROOT = os.path.dirname(os.path.dirname(os.path.dirname(os.path.abspath(__file__))))
SRC = os.path.join(ROOT, 'reference-html')
OUT = os.path.join(ROOT, 'app')

CHAPTERS = [
    (1, 'ch1-perraliere-v2.html', '7rWyskrP1nNicELmFbLMC5'),
    (2, 'ch2-tete-d-or.html', 'B4FQgktwnwBqSDzZYFJi3t'),
    (3, 'ch3-gratte-ciel.html', '5CpdYSS6fjRWYEpeb3DKne'),
    (4, 'ch4-mini-world.html', '8o1Q9vTUEAK9GmHMT7Qm7i'),
    (5, 'ch5-raptor-parc.html', 'MTiXmS6bj9vTU4HwL5PQas'),
    (6, 'ch6-anse.html', 'S5fDjWzhioZaiTuuKHJiCp'),
    (7, 'ch7-confluences.html', '5KwrNnvPLTCV3tfwPfBWxG'),
    (8, 'ch8-chez-julien.html', '2DdS7B9GpNXVVKLR3bxGs6'),
    (9, 'ch9-grande-fete.html', None),
]
LINKS = {aid: n for n, _, aid in CHAPTERS if aid}

BRIDGE = """
<script>
/* Relais de l'app Ulysse, Sacha et Bibou (ajouté à l'assemblage, le chapitre n'est pas modifié) */
(function(){
  var N=%(n)d, LINKS=%(links)s;
  function post(m){try{if(window.parent&&window.parent!==window)window.parent.postMessage(Object.assign({ulysse:1},m),'*');}catch(e){}}
  document.addEventListener('click',function(e){
    var a=e.target&&e.target.closest?e.target.closest('a[href*="claude.ai/artifact/"]'):null;if(!a)return;
    var id=(a.getAttribute('href').split('/artifact/')[1]||'').split(/[?#]/)[0];
    if(LINKS[id]&&window.parent!==window){e.preventDefault();e.stopPropagation();post({type:'goto',ch:LINKS[id]});}
  },true);
  var end=document.getElementById('end');
  if(end){var sent=false;new MutationObserver(function(){if(!end.hidden&&!sent){sent=true;post({type:'fin',ch:N});}if(end.hidden)sent=false;})
    .observe(end,{attributes:true,attributeFilter:['hidden']});}
  post({type:'ready',ch:N});
})();
</script>
"""

def main():
    os.makedirs(os.path.join(OUT, 'chapitres'), exist_ok=True)
    for n, src, _ in CHAPTERS:
        html = open(os.path.join(SRC, src), encoding='utf-8').read()
        bridge = BRIDGE % {'n': n, 'links': json.dumps(LINKS)}
        i = html.rfind('</body>')
        html = html[:i] + bridge + html[i:] if i >= 0 else html + bridge
        open(os.path.join(OUT, 'chapitres', 'ch%d.html' % n), 'w', encoding='utf-8').write(html)
        print('ch%d <- %s (%d Ko)' % (n, src, len(html.encode('utf-8')) // 1024))

if __name__ == '__main__':
    main()
