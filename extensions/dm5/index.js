const host = "https://m.dm5.com";

function home(page) {
    const result = fetch(`${host}/dm5.ashx`, {
        method: 'POST',
        body: {
            action: "getclasscomics",
            pageindex: page + 1,
            pagesize: 20,
            categoryid: 0,
            tagid: 0,
            status: 0,
            usergroup: 0,
            pay: -1,
            areaid: 0,
            sort: 2,
            iscopyright: 0,
        },
        bodyType: 'UrlEncoded'
    });
    const text = result.text();
    const json = JSON.parse(text);
    const items = json.UpdateComicItems.map(it => {
        return {
            id: it.Url,
            title: it.Title,
            thumb: it.ShowPicUrlB,
            extra: JSON.stringify(it),
        };
    });
    return items;
}

function search(keyword, page) {
    const result = fetch(`${host}/pagerdata.ashx`, {
        method: 'POST',
        body: {
            t: 7,
            pageindex: page + 1,
            f: 0,
            title: keyword
        },
        bodyType: 'UrlEncoded'
    });
    const text = result.text();
    const json = JSON.parse(text);
    const items = json.map(it => {
        return {
            id: it.UrlKey,
            title: it.Title,
            thumb: it.Pic,
            extra: JSON.stringify(it),
        };
    });
    return items;
}

function detail(gallery) {
    const extra = JSON.parse(gallery.extra);
    const url = extra.Url || extra.UrlKey;
    const response = fetch(`${host}/${url}`);
    const html = response.text();
    const doc = parseHtml(html);
    return {
        title: doc.querySelector('.detail-main-info-title').text(),
        thumb: doc.querySelector('.detail-main-cover > img').attr('src'),
        desc: doc.querySelector('.detail-desc').text(),
        chapters: doc.querySelectorAll('#detail-list-select-1>li').map(it => {
            return {
                id: it.querySelector('a').attr('href'),
                title: (it.querySelector('.detail-list-2-info-title') || it.querySelector('a')).text(),
                extra: JSON.stringify({
                    link: it.querySelector('a').attr('href'),
                    isLocked: it.querySelector('.detail-list-2-info-right') !== null,
                }),
            };
        })
    };
}

function canReadChapter(chapter) {
    const extra = JSON.parse(chapter.extra);
    return !extra.isLocked;
}

function loadChapterImages(chapter) {
    const extra = JSON.parse(chapter.extra);
    const link = `${host}${extra.link}`;
    const response = fetch(link);
    const html = response.text();
    const packed = html.match(/\(function\(p,a,c,k,e,d\).*?0,\{\}\)\)/)[0];
    const scriptResult = unpack(packed);
    const newImgs = scriptResult.substring(scriptResult.indexOf('[')).replace(/^;+|;+$/g, '').replace(/'/g, '"');
    const items = JSON.parse(newImgs);
    return items.map(it => it + "&bookId=" + extra.link.replace(/\//g, ''));
}

function canModifyRequest(uri) {
    return uri.includes('dm5.com') && uri.includes('&bookId=');
}

function modifyRequest(request) {
    if (request.uri.includes('&bookId=')) {
        const bookId = request.uri.substring(request.uri.indexOf('&bookId=') + '&bookId='.length);
        request.header = { Referer: `${host}/${bookId}/` };
    }
    return request;
}