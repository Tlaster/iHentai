const host = "https://exhentai.org/";
const cookie_key = 'exhentai_cookie';

const login = async (username, password) => {
    const response = await fetch('http://forums.e-hentai.org/index.php?act=Login&CODE=01&CookieDate=1', {
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
    const newCookie = await updateCookie(cookie);
    cookie += (';' + newCookie)
    setCookie(cookie);
    return 200;
}

const updateCookie = async (cookie) => {
    const response = await fetch('https://exhentai.org/mytags', {
        headers: {
            Cookie: `${cookie};igneous=`
        }
    });
    if (!response.headers.has('Set-Cookie')) {
        return '';
    }
    return response.headers.get('Set-Cookie');
}

const requireLogin = () => {
    const cookie = getCookies();
    return cookie === undefined || cookie === null;
}

const home = async (page) => {
    let uri = `${host}watched`;
    if (page !== 0) {
        uri += `?page=${page}`
    }
    const response = await fetch(uri);
    const html = await response.text();
    const doc = parseHtml(html);
    let items = doc.querySelectorAll('.itg.gltc tr:not(:first-child)');
    if (items.length === 0) {
        items = doc.querySelectorAll('.itg.gltm tr:not(:first-child)');
    }
    return items.map(it => {
        return {
            id: it.querySelector('.glname a').attr('href'),
            title: it.querySelector('.glink').text(),
            thumb: it.querySelector('.glthumb img').attr('data-src') || it.querySelector('.glthumb img').attr('src'),
            extra: JSON.stringify({
                link: it.querySelector('.glname a').attr('href'),
            }),
        }
    });
}

const detail = async (gallery) => {
    const extra = JSON.parse(gallery.extra);
    const url = extra.link;
    return await detailFromLink(url);
}

const loadGalleryImagePages = async (gallery) => {
    const extra = JSON.parse(gallery.extra);
    const pages = extra.pages;
    const result = await awaitAll(pages.map(it => detailFromLink(it)));
    debug.log(JSON.stringify(result));
    return flatMap(result.map(it => it.images)).map(it => it.source);
}

const loadImageFromPage = async (page) => {

}

const detailFromLink = async (url) => {
    const response = await fetch(url);
    const html = await response.text();
    const doc = parseHtml(html);
    const large = doc.querySelectorAll('.gdtl');
    const medium = doc.querySelectorAll('.gdtm');
    let img = [];
    if (large.length === 0) {
        img = medium.map(it => {
            return {
                crop: true,
                source: `${it.querySelector('div').attr('style')}`.match(/url\(([^\s]*)\)/)[1],
                offset_x: `${it.querySelector('div').attr('style')}`.match(/url\(([^\s]*)\) ((-)?\d+)(px)?/)[2],
                offset_y: `${it.querySelector('div').attr('style')}`.match(/url\(([^\s]*)\) ((-)?\d+)(px)? ((-)?\d+)(px)?/)[5],
                thumb_height: parseInt(`${it.querySelector('img').attr('style')}`.match(/height:(\d+)/)[1]),
                thumb_width: parseInt(`${it.querySelector('img').attr('style')}`.match(/width:(\d+)/)[1]),
                text: it.querySelector('img').attr('alt'),
            };
        })
    } else {
        img = large.querySelectorAll('.gdtl').map(it => {
            return {
                source: it.querySelector('img').attr('src'),
                text: it.querySelector('img').attr('alt'),
            };
        })
    }
    return {
        title: doc.querySelector('#gn').text(),
        sub_title: doc.querySelector('#gj').text(),
        thumb: `${doc.querySelector('#gd1 > div').attr('style')}`.match(/url\(([^\s]*)\)/)[1],
        has_rating: true,
        max_rating: 5,
        rating: parseFloat(doc.querySelector('#rating_label').text().match(/(\d+(\.\d+)?)/)[1]),
        images: img,
        tags: doc.querySelectorAll('#taglist > table > tbody > tr').map(it => {
            return {
                title: it.querySelector('.tc').text(),
                values: it.querySelectorAll('td:nth-child(2) > div').map(v => {
                    return {
                        value: v.querySelector('a').text(),
                        extra: v.querySelector('a').attr('href'),
                    };
                }),
            };
        }),
        extra: JSON.stringify({
            pages: [...doc.querySelectorAll('.ptt td:not(:first-child):not(:last-child) > a').map(it => it.attr('href'))],
        })
    }
}

const canModifyRequest = (uri) => {
    const cookie = getCookies();
    if (cookie === undefined || cookie === null) {
        return false;
    }
    return uri.includes('exhentai.org') || uri.includes('e-hentai.org');
}

const modifyRequest = (request) => {
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

const flatMap = (f, arr) => arr.reduce((x, y) => [...x, ...f(y)], [])
