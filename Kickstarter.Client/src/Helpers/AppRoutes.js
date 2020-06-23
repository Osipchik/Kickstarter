export const AppRoutes = {
    Home: '/',
    Company: '/Company/:id',
    CompanyPage: '/CompanyPage/:id',
    Explore: '/Explore/:categoryId?/:subcategoryId?',
    CreateCompany: '/CreateCompany/:companyId/:page?',
    Preview: '/Preview/:page?',
    Login: '/LogIn',
    SignUp: '/SignUp'
}


const ApiOrigin = 'https://localhost:4001';
const ApiVersion = '/api/v1';

const Endpoints = {
    Preview: ApiOrigin + ApiVersion + '/Preview',
    Company: ApiOrigin + ApiVersion + '/Company',

    Category: ApiOrigin + ApiVersion + '/Category',

    Funding: ApiOrigin + ApiVersion + '/Funding'
}

export const PreviewEndpoints = {

    GetPreviews: Endpoints.Preview + '/GetPreviews?',
    GetPreview: Endpoints.Preview + '/GetById?',
    GetUserPreviews: Endpoints.Preview + '/GetUserPreviews?userId=',
    
    UpdatePreviewInfo: Endpoints.Preview + '/UpdatePreview',
    UpdatePreviewImage: Endpoints.Preview + '/UpdatePreviewImage?previewId=',
    
    DeletePreviewImage: Endpoints.Preview + '/DeleteImage'
}

export const CompanyEndpoint = {
    CreateCompany: Endpoints.Company + '/CreateCompany'
}

export const CategoryEndpoint = {
    GetCategories: Endpoints.Category + '/GetCategories'
}

export const FundingEndpoint = {
    GetFunding: Endpoints.Funding + '/GetFunding?',
    Update: Endpoints.Funding + '/UpdateFunding',
}