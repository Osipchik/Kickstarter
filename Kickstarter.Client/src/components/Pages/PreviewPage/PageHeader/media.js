import React from "react";
import {GetYouTubeUrl} from "../../../methods";

export default function CompanyMedia(url){
    
    if (GetYouTubeUrl(url)) {
        return (
            <div className="embed-responsive embed-responsive-16by9 story-media pt-2">
                <iframe className="embed-responsive-item" src={url} allowFullScreen/>
            </div>
        )
    }
    else if (url){
        console.log('sad');
        return (
            <div className="story-media">
                <img src={url} alt="asd"/>
            </div>
        )
    }
    else {
        return (
            <div className="story-media pt-2">
                <img src="https://ksr-ugc.imgix.net/missing_project_photo.png?ixlib=rb-2.1.0&crop=faces&w=1552&h=873&fit=crop&v=&auto=format&frame=1&q=92&s=2ac7092cb1cf9c088e7cbf7f14676ab8" alt="asd"/>
            </div>
        )
    }
}