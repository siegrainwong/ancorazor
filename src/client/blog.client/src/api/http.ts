import axios from 'axios'

var host = "http://123.206.33.109:8012/api"

// TODO: holy shit I need TS course
function toType(obj) {
    return ({}).toString.call(obj).match(/\s([a-zA-Z]+)/)[1].toLowerCase()
}