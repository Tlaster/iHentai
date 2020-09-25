const host = "https://exhentai.org/";
const cookie_key = 'exhentai_cookie';

function login(username, password) {
    const response = fetch('http://forums.e-hentai.org/index.php?act=Login&CODE=01&CookieDate=1', {
        method: 'POST',
        body: {
            UserName: username,
            PassWord: password,
        },
        bodyType: 'UrlEncoded'
    });
    if (!response.headers.has('Set-Cookie')) {
        return 403;
    }
    let cookie = response.headers.get('Set-Cookie');
    const newCookie = updateCookie(cookie);
    cookie += (';' + newCookie);
    setCookie(cookie);
    return 200;
}

function updateCookie(cookie) {
    const response = fetch('https://exhentai.org/mytags', {
        headers: {
            Cookie: `${cookie};igneous=`
        }
    });
    if (!response.headers.has('Set-Cookie')) {
        return '';
    }
    return response.headers.get('Set-Cookie');
}

function requireLogin() {
    const cookie = getCookies();
    return cookie === undefined || cookie === null;
}

function home(page) {
    let uri = `${host}watched`;
    if (page !== 0) {
        uri += `?page=${page}`;
    }
    const response = fetch(uri);
    const html = response.text();
    const doc = parseHtml(html);
    let items = doc.querySelectorAll('.itg.gltc tr:not(:first-child)');
    if (items.length === 0) {
        items = doc.querySelectorAll('.itg.gltm tr:not(:first-child)');
    }
    return items.map(it => ({
        id: it.querySelector('.glname a').attr('href'),
        title: it.querySelector('.glink').text(),
        thumb: it.querySelector('.glthumb img').attr('data-src') || it.querySelector('.glthumb img').attr('src'),
        extra: JSON.stringify({
            link: it.querySelector('.glname a').attr('href'),
        }),
    }));
}

function detail(gallery) {
    const extra = JSON.parse(gallery.extra);
    const url = extra.link;
    return detailFromLink(url);
}

function loadGalleryImagePages(gallery) {
    const extra = JSON.parse(gallery.extra);
    const pages = extra.pages;
    const list = [];
    const result = pages.map(it => detailFromLink(it));
    result.forEach(element => {
        if (element) {
            element.images.map(it => it.link).forEach(it => {
                list.push(it);
            });
        }
    });
    return list;
}

function loadImageFromPage(page) {
    const response = fetch(page);
    const html = response.text();
    const doc = parseHtml(html);
    return doc.querySelector('#img').attr('src');
}

function detailFromLink(url) {
    const response = fetch(url);
    const html = response.text();
    const doc = parseHtml(html);
    const large = doc.querySelectorAll('.gdtl');
    const medium = doc.querySelectorAll('.gdtm');
    let img = [];
    if (large.length === 0) {
        img = medium.map(it => ({
            crop: true,
            source: `${it.querySelector('div').attr('style')}`.match(/url\(([^\s]*)\)/)[1],
            offset_x: `${it.querySelector('div').attr('style')}`.match(/url\(([^\s]*)\) ((-)?\d+)(px)?/)[2],
            offset_y: `${it.querySelector('div').attr('style')}`.match(/url\(([^\s]*)\) ((-)?\d+)(px)? ((-)?\d+)(px)?/)[5],
            thumb_height: parseInt(`${it.querySelector('img').attr('style')}`.match(/height:(\d+)/)[1]),
            thumb_width: parseInt(`${it.querySelector('img').attr('style')}`.match(/width:(\d+)/)[1]),
            text: it.querySelector('img').attr('alt'),
            link: it.querySelector('a').attr('href'),
        }));
    } else {
        img = large.querySelectorAll('.gdtl').map(it => ({
            source: it.querySelector('img').attr('src'),
            text: it.querySelector('img').attr('alt'),
            link: it.querySelector('a').attr('href'),
        }));
    }
    return {
        title: doc.querySelector('#gn').text(),
        sub_title: doc.querySelector('#gj').text(),
        thumb: `${doc.querySelector('#gd1 > div').attr('style')}`.match(/url\(([^\s]*)\)/)[1],
        has_rating: true,
        max_rating: 5,
        rating: parseFloat(doc.querySelector('#rating_label').text().match(/(\d+(\.\d+)?)/)[1]),
        images: img,
        meta: doc.querySelectorAll('#gdd > table > tbody > tr').map(it => ({ [it.querySelector('.gdt1').text()]: it.querySelector('.gdt2').text() })).reduce((obj, item) => Object.assign(obj, { ...item }), {}),
        tags: doc.querySelectorAll('#taglist > table > tbody > tr').map(it => ({
            title: it.querySelector('.tc').text(),
            values: it.querySelectorAll('td:nth-child(2) > div').map(v => ({
                value: v.querySelector('a').text(),
                extra: v.querySelector('a').attr('href'),
            })),
        })),
        comments: doc.querySelectorAll('.c1').map(it => ({
            content: it.querySelector('.c6').html(),
            user: it.querySelector('.c3 > a').text(),
            created_at: `${it.querySelector('.c3').text()}`.match(/Posted on (.*) by:/)[1],
        })),
        extra: JSON.stringify({
            pages: [...doc.querySelectorAll('.ptt td:not(:first-child):not(:last-child) > a').map(it => it.attr('href'))],
        })
    };
}

function canModifyRequest(uri) {
    const cookie = getCookies();
    if (cookie === undefined || cookie === null) {
        return false;
    }
    return uri.includes('exhentai.org') || uri.includes('e-hentai.org');
}

function modifyRequest(request) {
    const cookie = getCookies();
    request.header = { Cookie: cookie };
    return request;
}

function getCookies() {
    return localStorage.getItem(cookie_key);
}

function setCookie(cookie) {
    localStorage.setItem(cookie_key, cookie);
}

function flatMap(f, arr) {
    return arr.reduce((x, y) => [...x, ...f(y)], []);
}
