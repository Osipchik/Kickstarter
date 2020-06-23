import {
    UPDATE_CATEGORY_ID, 
    UPDATE_DESCRIPTION,
    UPDATE_PROJECT_IMAGE, 
    UPDATE_PROJECT_VIDEO_URL,
    UPDATE_SUB_CATEGORY_ID,
    UPDATE_TITLE,
    LOAD_COMPANY
} from "../actionTypes";

export const updateTitle = (title) => {
    return{
        type: UPDATE_TITLE,
        payload: title
    }
};

export const updateDescription = (description) => {
    return{
        type: UPDATE_DESCRIPTION,
        payload: description
    }
};

export const updateCategoryId = (id) => {
    return{
        type: UPDATE_CATEGORY_ID,
        payload: id
    }
};

export const updateSubCategoryId = (id) => {
    return{
        type: UPDATE_SUB_CATEGORY_ID,
        payload: id
    }
};

export const updateProjectImage = (src, file) => {    
    return{
        type: UPDATE_PROJECT_IMAGE,
        payload: {url: src, file: file}
    }
};

export const updateProjectVideo = (videoUrl) => {
    return{
        type: UPDATE_PROJECT_VIDEO_URL,
        payload: videoUrl
    }
};

export const loadBasic = (id) => {
    return{
        type: LOAD_COMPANY,
        payload: id
    }
};