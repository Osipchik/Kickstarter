import {
    UPDATE_CATEGORY_ID,
    UPDATE_DESCRIPTION,
    UPDATE_PROJECT_IMAGE, 
    UPDATE_PROJECT_VIDEO_URL,
    UPDATE_SUB_CATEGORY_ID,
    UPDATE_TITLE,
    LOAD_COMPANY
} from "../actionTypes";

const initialState = {
    title: '',
    description: '',
    categoryId: 0,
    subCategoryId: 0,
    previewImage: {
        url: '',
        file: null
    },
    videoUrl: '',

    id: null
};

const BasicsPageReducer = (state = initialState, action) => {
    switch (action.type) {
        case UPDATE_TITLE: return {
            ...state,
            title: action.payload
        };
        case UPDATE_DESCRIPTION: return {
            ...state,
            description: action.payload
        };
        case UPDATE_CATEGORY_ID: return {
            ...state,
            categoryId: action.payload
        };
        case UPDATE_SUB_CATEGORY_ID: return {
            ...state,
            subCategoryId: action.payload
        };
        case UPDATE_PROJECT_IMAGE: return {
            ...state,
            previewImage: action.payload
        };
        case UPDATE_PROJECT_VIDEO_URL: return {
            ...state,
            videoUrl: action.payload
        };
        case LOAD_COMPANY: return {
            ...state,
            id: action.payload
        };
        default: return {...state}
    }
};

export default BasicsPageReducer