import { createUserManager } from 'redux-oidc';
import { WebStorageStateStore } from 'oidc-client';

const userManagerConfig = {
    userStore: new WebStorageStateStore({ store: localStorage }) ,
    authority: 'https://localhost:5000',
    client_id: 'jsClient',
    redirect_uri: 'https://localhost:3000/auth-callback',
    post_logout_redirect_uri: 'https://localhost:3000/',
    silent_redirect_uri: 'https://localhost:3000/silent_renew.html',
    response_type: "code",
    scope: "openid profile offline_access kickstarterGateway",
    automaticSilentRenew: true,
    filterProtocolClaims: true,
    loadUserInfo: true,
    
};
  
const userManager = createUserManager(userManagerConfig);
  
export default userManager;