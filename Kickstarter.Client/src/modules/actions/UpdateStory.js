import { UPDATE_EDITOR, UPDATE_RISKS, LOAD_COMPANY } from "../actionTypes";

export const updateEditor = (editorState) => {
    return {
        type: UPDATE_EDITOR,
        payload: editorState
    }
};

export const updateRisks = risks => {
    return {
        type: UPDATE_RISKS,
        payload: risks,
    }  
};

export const loadStory = (id) => {
    return{
        type: LOAD_COMPANY,
        payload: id
    }
};