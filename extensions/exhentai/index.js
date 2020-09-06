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
    debug.log(cookie);
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