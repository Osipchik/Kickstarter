import { createStore, applyMiddleware } from "redux";
import rootReducer from './reducers'
import { loadUser } from "redux-oidc";
import userManager from '../auth/userManager';
import Redux from "redux-thunk" 

const initialState = {};

const store = createStore(rootReducer, initialState);
loadUser(store, userManager);

export default store;