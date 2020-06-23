import { combineReducers } from "redux";
import BasicsPageReducer from "./BasicsPageReducer";
import FoundingPageReducer from "./FoundingPageReducer";
import StoryReducer from "./StoryReducer";
import UserCompaniesReducer from "./UserCompaniesReducer"
import { reducer as oidcReducer } from 'redux-oidc';

export default combineReducers({
    oidc: oidcReducer,
    BasicsPageReducer, 
    FoundingPageReducer, 
    StoryReducer,
    UserCompaniesReducer
});