namespace Gasconade.UiCore
{
    /// <summary>
    /// Default script and style definitions if the user does not provide their own
    /// </summary>
    public static class DefaultContent {
        /// <summary>
        /// Javascript to be embedded on Gasconade page
        /// </summary>
        public const string DefaultJavaScript = @"
function blockToggle(mouseEvt) {
    var trg = document.getElementById('expand_' + mouseEvt.target.id);
    trg.style.display = (trg.style.display == 'block') ? 'none' : 'block';
}
(function (){
    var all = document.getElementsByClassName('header');
    for(var i=0; i<all.length;i++){
        all[i].onclick = blockToggle;  
    }
})();
";

        /// <summary>
        /// CSS to be embedded on Gasconade page
        /// </summary>
        public const string DefaultStyleSheet = @"
.header { cursor: pointer; }
.template {
    padding: 1em 2em;
    font-weight: bold;
    font-family: monospace;
    background-color: #ddf;
    display: block;
}
.obsoleteTemplate {
    padding: 1em 2em;
    font-family: monospace;
    background-color: #fdd;
    display: block;
}
.expando {
    display:none;
}
.titleNote {
    float: right;
    margin-right: 1em;
}
.MessageBlock:nth-child(odd) {
    background-color: #fff;
    margin: 10;
}
.MessageBlock:nth-child(even) {
    background-color: #eee;
    margin: 10;
}
h1, h2, h4 {
    margin: 1em 0 0.5em;
    line-height: 1.25;
}
h3 {
    padding: 0.5em;
    margin: 0;
    background-color: #ddd;
}
h3:hover { background-color: #ccc; }
h3:active { background-color: #bbb; }
body {
    margin: 0;
    padding: 0;
    font-size: 100%;
    line-height: 1.5;
    font-family: sans-serif;
}
div { display: block; }
dt { font-weight: bold; font-size: 80%; }
dd { font-size: 80%; }
";
    }
}