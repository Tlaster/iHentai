const host = "https://m.manhuagui.com";

function home(page) {
    const result = fetch(`${host}/update/?page=${page + 1}&ajax=1&order=1`);
    const html = result.text();
    const doc = parseHtml(html);
    return doc.querySelectorAll('body>li').map((it) => ({
        id: it.querySelector('a').attr('href'),
        title: it.querySelector('h3').text(),
        thumb: it.querySelector('.thumb > img').attr('data-src'),
        extra: JSON.stringify({
            link: it.querySelector('a').attr('href'),
        }),
    }));
}

function detail(gallery) {
    const extra = JSON.parse(gallery.extra);
    const url = extra.link;
    const response = fetch(`${host}${url}`);
    const html = response.text();
    const doc = parseHtml(html);
    return {
        title: doc.querySelector('.main-bar > h1').text(),
        thumb: doc.querySelector('.thumb > img').attr('src'),
        desc: doc.querySelector('#bookIntro').text(),
        chapters: doc.querySelectorAll('#chapterList>ul>li').map((it) => ({
            id: it.querySelector('a').attr('href'),
            title: it.querySelector('a > b').text(),
            extra: JSON.stringify({
                link: it.querySelector('a').attr('href')
            }),
        }))
    };
}

function loadChapterImages(chapter) {
    const extra = JSON.parse(chapter.extra);
    const link = `${host}${extra.link}`;
    const response = fetch(link);
    const html = response.text();
    const packed = html.match(/\(function\(p,a,c,k,e,d\).*?0,\{\}\)\)/)[0];
    const packges = packed.split(',');
    const replaceable = [...packges][packges.length - 3];
    const base64LZString = replaceable.split('\'')[1];
    const script = packed.replace(replaceable, `'${decodeLzStringFromBase64(base64LZString)}'.split('|')`);
    const scriptResult = unpack(script);
    const data = JSON.parse(scriptResult.substring(11, scriptResult.indexOf(').preInit();')));
    const files = data.files || data.images;
    return files.map((it) => `https://i.hamreus.com${it}?cid=${data.chapterId}&e=${data.sl.e}&m=${data.sl.m}&bookId=${data.bookId}`);
}

function canModifyRequest(uri) {
    return uri.includes('i.hamreus.com') && uri.includes('bookId') && uri.includes('cid');
}

function modifyRequest(request) {
    if (request.uri.includes('bookId') && request.uri.includes('cid')) {
        const bookId = getParameterByName('bookId', request.uri);
        const chapterId = getParameterByName('cid', request.uri);
        request.header = { Referer: `${host}/comic/${bookId}/${chapterId}.html` };
    }
    return request;
}

function getParameterByName(name, url) {
    name = name.replace(/[\[\]]/g, '\\$&');
    const regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, ' '));
}
