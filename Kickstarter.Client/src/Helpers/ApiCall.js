import store from '../modules/store';
import { CompanyEndpoint } from './AppRoutes';


export const GetAuthHeader = () => {
    const token = store.getState().oidc.user.access_token;
    const headers = new Headers();
    headers.append('Accept', 'application/json');
    headers.append('Authorization', `Bearer ${token}`);

    return headers;
}

export const CreateRequestOptions = (data, method = 'get', isAuthenticate = false) => {
    let options = {
        method: method,
        body: JSON.stringify(data)
    }

    if (isAuthenticate) {
        options.headers = GetAuthHeader();
        options.headers.append('Content-Type', 'application/json');
    }

    return options;
}

export const CreateRequestImageOptions = (image, method) => {
    const formData = new FormData()
    formData.append('previewImage', image)

    let options = {
        method: method,
        headers: GetAuthHeader(),
        body: formData
    }

    return options;
}

export const ApiFetch = async (url, options) => {

    try {
        let response = await fetch(url, options);
        if (response.ok) {
            return 'json' in response ? await response.json() : response.status;
        }
    
        console.log(`error: ${url}`, `status: ${response.status}`);
    } catch (error) {
        console.log(error);
    }
}

export const CreateCompany = async () => {
    let options = CreateRequestOptions(null, 'post', true);
    let response = await ApiFetch(CompanyEndpoint.CreateCompany, options);

    return response;
}