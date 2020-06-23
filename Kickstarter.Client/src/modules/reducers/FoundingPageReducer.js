import {
    UPDATE_CHECKED,
    UPDATE_END_DATE, 
    UPDATE_GOAL, 
    UPDATE_LAUNCH_DATE,
    LOAD_COMPANY
} from "../actionTypes";
import moment from "moment";

const initialState = {
    goal: 0,
    launchDate: moment().add(30, "days"),
    endDate: moment().add(30, "days"),
    
    id: null
};

const FoundingPageReducer = (state = initialState, action) => {
    switch (action.type) {
        case UPDATE_LAUNCH_DATE: return {
            ...state,
            launchDate: action.payload
        };
        case UPDATE_END_DATE: return {
            ...state,
            endDate: action.payload
        };
        case UPDATE_GOAL: return {
            ...state,
            goal: action.payload
        };
        case UPDATE_CHECKED: return {
            ...state,
            checked: action.payload
        };
        case LOAD_COMPANY: return {
            ...state,
            id: action.payload
        };
        default: return {...state}
    }
};

export default FoundingPageReducer