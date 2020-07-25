import axios from "axios";
import * as constants from "../constants"

export default axios.create({
    baseURL: constants.API_URL,
    responseType: "json"
});