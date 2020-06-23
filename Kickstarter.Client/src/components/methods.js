import moment from 'moment';


/**
 * @return {number}
 */

export function GetDatesDifference(end) {
    let date = end;
    if(typeof(end) === typeof('')){
        date = moment(end);
    }
    return Math.ceil(moment.duration(date.diff(moment())).asDays());
}



/**
 * @return {boolean}
 */

export function CheckNum(num, maxVal, minVal = 0) {
    return num >= minVal && num <= maxVal
}


/**
 * @return {string}
 */

export function GetYouTubeUrl(link){
    const youTube = 'https://www.youtube.com/';
    const watch = 'watch?v=';
    const embed = 'embed/';

    let url = undefined;
    if (link !== undefined){
        if(link.includes(youTube + embed)){
            url = link;
        }
        else if(link.includes(youTube + watch)){
            url = link.replace(watch, embed);
            url = url.includes('&') ? url.slice(0, url.indexOf('&') - url.length) : url;
        }
    }
    
    return url;
}

/**
 * @return {string}
 */
export function GetLocalNum(num, culture) {
    return new Intl.NumberFormat(culture ? culture : window.navigator.language).format(num);
}
