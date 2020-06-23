import React, { useState, useEffect } from 'react'
import '../../../../styles/Text.css'
import {CustomButton} from "../../../Decorators/Buttons";
import CompanyMedia from "./media";
import Social from "./Social";
import ProjectProgress from "./ProjectProgress";
import { PreviewEndpoints } from '../../../../Helpers/AppRoutes';


export const CompanyHeader = (props) => {
    console.log(props)

    const [title, setTitle] = useState(props.title);
    const [description, setDescription] = useState(props.description);
    const [media, setMedia] = useState(props.media);
    const [funded, setFunded] = useState(0);
    const [goal, setGoal] = useState(props.goal);
    const [date, setDate] = useState(props.id !== undefined ? '' : props.endDate.format('LLLL'));

    const defaultImage = 'https://ksr-ugc.imgix.net/missing_project_photo.png?ixlib=rb-2.1.0&crop=faces&w=1552&h=873&fit=crop&v=&auto=format&frame=1&q=92&s=2ac7092cb1cf9c088e7cbf7f14676ab8';

    useEffect(() => {
        const loadPage = async () => {
            let params = new URLSearchParams({
                id: props.id,
                auth: false,
            });
            let response = await fetch(PreviewEndpoints.GetPreview + params.toString(),{
                method: 'get'
            });
    
            let result = await response.json();
            console.log(result)

            setTitle(result.title);
            setDescription(result.description);
            setMedia(getMedia(result.imageUrl, result.videoUrl));
            setFunded(result.founded);
            setGoal(result.goal);
            setDate(result.endDate);
        }

        if (props.id !== undefined){
            loadPage();
        }
    }, [])

    const getMedia = (imageUrl, videoUrl) => {
        let media = videoUrl === null || videoUrl === '' ? imageUrl : videoUrl;
        console.log(imageUrl);
        console.log(videoUrl);

        return media === null || media === '' ? defaultImage : media;
    }

    const Titles = (
        <div className="m-header">
            <br/>
            <h4 className="title">{title}</h4>
            <p className="sub-title font">{description}</p>
            <br/>
        </div>
    );

    return(
        <div className="text-center">
            <br/>
            <div className="hide-on-md">
                {Titles}
            </div>
            <div className="row">
                {CompanyMedia(media)}
                <div className="col px18 mx-auto">
                    <div className="show-on-md">
                        {Titles}
                    </div>
                    <ProjectProgress 
                        duration={date}
                        founded={funded}
                        goal={goal}/>
                    <br/>
                    <CustomButton variant="contained" className="w-100">
                        Back this project
                    </CustomButton>
                    <Social />
                    <p className="text-small text-left">
                        All or nothing. This project will only be funded if it reaches its goal by {date}.
                    </p>
                </div>
            </div>
        </div>
    );
}