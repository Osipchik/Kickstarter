import {
    UPDATE_CHECKED,
    UPDATE_END_DATE, 
    UPDATE_GOAL, 
    UPDATE_LAUNCH_DATE,
    LOAD_COMPANY
} from "../actionTypes";

export const updateLaunchDate = (launch) => {
    return{
        type: UPDATE_LAUNCH_DATE,
        payload: launch
    }
};

export const updateEndDate = (end) => {
    return{
        type: UPDATE_END_DATE,
        payload: end
    }
};

export const updateGoal = (goal) => {
    return{
        type: UPDATE_GOAL,
        payload: goal
    }
};

export const updateChecked = (checked) => {
    return{
        type: UPDATE_CHECKED,
        payload: checked
    }
};

export const loadFunding = (id) => {
    return{
        type: LOAD_COMPANY,
        payload: id
    }
};