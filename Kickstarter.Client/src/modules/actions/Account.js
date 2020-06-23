import { LOG_IN, LOG_OUT, SET_TOKENS, SET_USER } from "../actionTypes";

export const setTokens = (token) => {
    return {
        type: SET_TOKENS,
        accessToken: token.access_token,
        refreshToken: token.refresh_token
    }
}

export const login = (email, password) => {
    
}