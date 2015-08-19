function _getHash (hash) {
    if (hash.indexOf('#/') > -1) {
        hash = hash.substring(hash.indexOf('#/') + 2);
    } else if (hash.indexOf('#') > -1) {
        hash = hash.substring(1);
    }

    return hash;
};

function _deserialize (query) {
    var match,
        pl = /\+/g,  // Regex for replacing addition symbol with a space
        search = /([^&=]+)=?([^&]*)/g,
        decode = function (s) { return decodeURIComponent(s.replace(pl, ' ')); },
        obj = {};
    match = search.exec(query);
    while (match) {
        obj[decode(match[1])] = decode(match[2]);
        match = search.exec(query);
    }

    return obj;
};

if (window.location.hash.indexOf('id_token=') > -1) {

    var hash = _getHash(window.location.hash);
    var params = _deserialize(hash);
    if (params.hasOwnProperty('id_token')) {
        var token = params.id_token;

        var f = document.createElement("form");
        f.setAttribute('method', 'post');
        f.setAttribute('action', '/');
        f.setAttribute('id', 'auto');

        var i = document.createElement("input");
        i.setAttribute('type', 'hidden');
        i.setAttribute('name', 'id_token');
        i.setAttribute('id', 'id_token');
        i.setAttribute('value', token);

        f.appendChild(i);

        document.getElementsByTagName('body')[0].appendChild(f);
        document.getElementById('auto').submit();
    }
}

