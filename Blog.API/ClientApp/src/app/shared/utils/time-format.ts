export function getKeyByValue(object, value) {
  return Object.keys(object).find(key => object[key] === value);
}

export function timeFormat(time: Date) {
  function zeroize(num: number) {
    return (String(num).length == 1 ? "0" : "") + num;
  }

  let timestamp = time.valueOf() / 1000;

  let curTimestamp = new Date().getTime() / 1000; //当前时间戳
  let timestampDiff = curTimestamp - timestamp; // 参数时间戳与当前时间戳相差秒数

  let curDate = new Date(curTimestamp * 1000); // 当前时间日期对象
  let tmDate = new Date(timestamp * 1000); // 参数时间戳转换成的日期对象

  let Y = tmDate.getFullYear(),
    m = tmDate.getMonth() + 1,
    d = tmDate.getDate();
  let H = tmDate.getHours(),
    i = tmDate.getMinutes(),
    s = tmDate.getSeconds();

  if (timestampDiff < 60) {
    // 一分钟以内
    return "刚刚";
  } else if (timestampDiff < 3600) {
    // 一小时前之内
    return Math.floor(timestampDiff / 60) + "分钟前";
  } else if (
    curDate.getFullYear() == Y &&
    curDate.getMonth() + 1 == m &&
    curDate.getDate() == d
  ) {
    return zeroize(H) + ":" + zeroize(i);
  } else {
    let newDate = new Date((curTimestamp - 86400) * 1000); // 参数中的时间戳加一天转换成的日期对象
    if (
      newDate.getFullYear() == Y &&
      newDate.getMonth() + 1 == m &&
      newDate.getDate() == d
    ) {
      return zeroize(H) + ":" + zeroize(i);
    } else if (curDate.getFullYear() == Y) {
      return zeroize(H) + ":" + zeroize(i);
    } else {
      return zeroize(H) + ":" + zeroize(i);
    }
  }
}

export function dateFormat(date: Date) {
  function zeroize(num: number) {
    return (String(num).length == 1 ? "0" : "") + num;
  }

  let timestamp = date.valueOf() / 1000;

  let curTimestamp = new Date().getTime() / 1000; //当前时间戳

  let curDate = new Date(curTimestamp * 1000); // 当前时间日期对象
  let tmDate = new Date(timestamp * 1000); // 参数时间戳转换成的日期对象

  let Y = tmDate.getFullYear(),
    m = tmDate.getMonth() + 1,
    d = tmDate.getDate();

  if (
    curDate.getFullYear() == Y &&
    curDate.getMonth() + 1 == m &&
    curDate.getDate() == d
  ) {
    return "今天";
  } else {
    let newDate = new Date((curTimestamp - 86400) * 1000); // 参数中的时间戳加一天转换成的日期对象
    if (
      newDate.getFullYear() == Y &&
      newDate.getMonth() + 1 == m &&
      newDate.getDate() == d
    ) {
      return "昨天";
    } else if (curDate.getFullYear() == Y) {
      return zeroize(m) + "." + zeroize(d);
    } else {
      return Y + "\n" + zeroize(m) + "." + zeroize(d);
    }
  }
}
