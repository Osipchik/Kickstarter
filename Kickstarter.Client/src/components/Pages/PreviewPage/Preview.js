import React from 'react'
import { CompanyTabs } from './CompanyTabs/CompanyTabs';
import { useSelector } from "react-redux";


export const Preview = (props) => {
    const editor = useSelector(state => state.StoryReducer);
    const basic = useSelector(state => state.BasicsPageReducer);
    const founding = useSelector(state => state.FoundingPageReducer);

    return (
        <CompanyTabs
            isPreview={true}
            editor={editor}
            basic={basic}
            founding={founding}
        />
    )
}