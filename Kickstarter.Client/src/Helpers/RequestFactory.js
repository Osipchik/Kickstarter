export const RequestFactory = (type, params) => {

    if (type === types.GetPreviews) {
        return {
            url: `${type}?` + new URLSearchParams(params)
        }
    }
}

const types = {
    GetPreviews: 'GetPreviews',
    ddd: 'ddd'
}