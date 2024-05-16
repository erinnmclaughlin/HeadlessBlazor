window.headManipulation = {
    setTheme: function (theme) {
        document.documentElement.setAttribute('hb-theme', theme);
    },
    addLink: function (href, rel) {
        var link = document.createElement('link');
        link.href = href;
        link.rel = rel;
        document.head.appendChild(link);
    },
    removeLink: function (href) {
        console.log('removing link: ' + href);
        var links = document.head.getElementsByTagName('link');
        for (var i = 0; i < links.length; i++) {
            if (links[i].href === href) {
                document.head.removeChild(links[i]);
                break;
            }
        }
    },
    addScript: function (src) {
        var script = document.createElement('script');
        script.src = src;
        script.async = true;
        document.head.appendChild(script);
    },
    removeScript: function (src) {
        console.log('removing script: ' + src);
        var scripts = document.head.getElementsByTagName('script');
        for (var i = 0; i < scripts.length; i++) {
            if (scripts[i].src === src) {
                document.head.removeChild(scripts[i]);
                break;
            }
        }
    },
    removeStyle: function () {
        console.log('removing style');
        var s = document.head.getElementsByTagName('style');
        if (s) {
            for (var i = 0; i < s.length; i++) {
                document.head.removeChild(s[i]);
            }
        }
    }
};
