import React, { useState } from 'react';
import StoryPage from "../StoryPage/StoryPage";
import { Loading } from '../../../Decorators/Loading';


export const StoryTab = (props) => {
    const [loading, setLoading] = useState(props === null);
    const [story, setStory] = useState(props !== null ? props.story : '');
    const [risks, setRisks] = useState(props !== null ? props.risks : '');

    return (
        <div>
            {
                loading 
                    ? <Loading color="secondary"/>
                    : <StoryPage risks={risks} story={story}/>
            }
        </div>
    )
}