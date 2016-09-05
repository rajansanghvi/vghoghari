var PROTOCOL = window.location.protocol;
var URL = window.location.host;
var BASEURL = PROTOCOL + '//' + URL;

function getQueryStringValue(key) {
  return unescape(window.location.search.replace(new RegExp("^(?:.*[&\\?]" + escape(key).replace(/[\.\+\*]/g, "\\$&") + "(?:\\=([^&]*))?)?.*$", "i"), "$1"));
}