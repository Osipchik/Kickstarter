import { UPDATE_EDITOR, UPDATE_RISKS } from "../actionTypes";
import { EditorState } from 'draft-js';

const initialState = {
    editorState: EditorState.createEmpty(),
    risks: ''
};

const StoryReducer = (state = initialState, action) => {
    switch (action.type) {
        case UPDATE_EDITOR: return {
            ...state,
            editorState: action.payload
        };
        case UPDATE_RISKS: return {
            ...state,
            risks: action.payload
        };
        default: return {...state};
    }
};

export default StoryReducer