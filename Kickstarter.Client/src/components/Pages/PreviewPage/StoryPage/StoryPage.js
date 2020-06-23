import React from "react";
import Story from "./Story";
import '../../../../styles/Story.css'

export default function StoryPage(props) {
    const {story, risks} = props;
    
    return (
        <div className="row">
            <div className="col-2 max-w-8 max-h-12 hide-on-md">
                Story
                <hr/>
                <p>Risks and challenges</p>
            </div>
            <div className="max-w113 mx-auto col">
                <p className="title-smaller">Story</p>
                <Story story={story} risks={risks}/>
            </div>
            <div className="col-3 d-none d-md-block">
                user
            </div>
        </div>
    )
}