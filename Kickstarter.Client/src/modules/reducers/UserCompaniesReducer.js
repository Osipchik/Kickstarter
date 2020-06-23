import { SET_USER_COMPANIES, NEED_UPDATE_USER_COMPANIES } from "../actionTypes";

const initialState = {
    companies: [],
    needUpdate: true
};

const UserCompaniesReducer = (state = initialState, action) => {
    switch (action.type) {
        case SET_USER_COMPANIES: return {
            ...state,
            companies: action.payload,
            needUpdate: false
        };
        case NEED_UPDATE_USER_COMPANIES: return {
            ...state,
            needUpdate: action.payload
        };
        default: return {...state};
    }
};

export default UserCompaniesReducer