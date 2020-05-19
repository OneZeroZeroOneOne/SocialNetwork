
export function loadCss(href: string, media: string = 'all', attributes: any = {}) {
    let doc = window.document;
    let ss = doc.createElement("link");
    let refs = (doc.body || doc.getElementsByTagName("head")[0]).childNodes;
    let ref = refs[refs.length - 1];

    let sheets = doc.styleSheets;
    if (attributes) {
        for (var attributeName in attributes) {
            if (attributes.hasOwnProperty(attributeName)) {
                ss.setAttribute(attributeName, attributes[attributeName]);
            }
        }
    }

    ss.rel = "stylesheet";
    ss.href = href;
    ss.media = "only x";

    // wait until body is defined before injecting link. This ensures a non-blocking load in IE11.
    function ready(cb: any) {
        if (doc.body) {
            return cb();
        }
        setTimeout(function () {
            ready(cb);
        });
    }
    // Inject link
    // Note: the ternary preserves the existing behavior of "before" argument, but we could choose to change the argument to "after" in a later release and standardize on ref.nextSibling for all refs
    // Note: `insertBefore` is used instead of `appendChild`, for safety re: http://www.paulirish.com/2011/surefire-dom-element-insertion/
    ready(function () {
        if (ref === null || ref.parentNode === null)
            return;
        ref.parentNode.insertBefore(ss, ref);
    });
    // A method (exposed on return object for external use) that mimics onload by polling document.styleSheets until it includes the new sheet.
    let onloadcssdefined = function (cb) {
        var resolvedHref = ss.href;
        var i = sheets.length;
        while (i--) {
            if (sheets[i].href === resolvedHref) {
                return cb();
            }
        }
        setTimeout(function () {
            onloadcssdefined(cb);
        });
    };

    function loadCB() {
        // @ts-ignore
        if (ss.addEventListener) {
            ss.removeEventListener("load", loadCB);
        }
        ss.media = media;
    }

    // once loaded, set link's media back to `all` so that the stylesheet applies once it loads
    if (ss.addEventListener) {
        ss.addEventListener("load", loadCB);
    }
    // @ts-ignore
    ss.onloadcssdefined = onloadcssdefined;
    onloadcssdefined(loadCB);
    return ss;
}

export function onloadCSS(ss: HTMLLinkElement, callback: Function) {
    let called: boolean;

    function newcb() {
        if (!called && callback) {
            called = true;
            callback.call(ss);
        }
    }
    if (ss.addEventListener) {
        ss.addEventListener("load", newcb);
    }
    // @ts-ignore
    if (ss.attachEvent) {
        // @ts-ignore
        ss.attachEvent("onload", newcb);
    }

    // This code is for browsers that don’t support onload
    // No support for onload (it'll bind but never fire):
    //	* Android 4.3 (Samsung Galaxy S4, Browserstack)
    //	* Android 4.2 Browser (Samsung Galaxy SIII Mini GT-I8200L)
    //	* Android 2.3 (Pantech Burst P9070)

    // Weak inference targets Android < 4.4
    if ("isApplicationInstalled" in navigator && "onloadcssdefined" in ss) {
        // @ts-ignore
        ss.onloadcssdefined(newcb);
    }
}