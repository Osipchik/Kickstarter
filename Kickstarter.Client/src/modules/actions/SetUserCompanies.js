import { SET_USER_COMPANIES, NEED_UPDATE_USER_COMPANIES } from "../actionTypes";

export const setUserCompanies = (companies) => {
    return{
        type: SET_USER_COMPANIES,
        payload: companies
    }
};

export const setNeedUpdateUserCompanies = (needUpdate) => {
    return{
        type: NEED_UPDATE_USER_COMPANIES,
        payload: needUpdate
    }
};