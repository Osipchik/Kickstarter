import Typography from "@material-ui/core/Typography";
import Box from "@material-ui/core/Box";
import {LineInput} from "../../../Decorators/TextInput/TextFields";
import React, {useState} from "react";
import PropTypes from "prop-types";
import Grow from "@material-ui/core/Grow";
import Paper from "@material-ui/core/Paper";
import YouTubeIcon from '@material-ui/icons/YouTube';
import Fade from "@material-ui/core/Fade";
import {GetYouTubeUrl} from "../../../methods";
import {VideoLearnMore} from "../LearnMore/LearnMore";
import '../../../../styles/Uploader.css'
import {ComponentTitle} from "../ComponentTitle";

export default function VideoCol(props) {
    const {videoUrl, onChange, className} = props;
    
    const [value, setValue] = useState(videoUrl);
    const [error, setError] = useState(false);
    const getVideoId = (link) => {
        setValue(link);
        setError(false);
        let url = GetYouTubeUrl(link);
        if(url){
            onChange(url);
            setError(false)
        }
        else{
            setError(true)
        }
    };
    
    const video = (
        <Fade in={true}>
            <div className="embed-responsive embed-responsive-16by9">
                <iframe
                    className="embed-responsive-item"
                    src={videoUrl}
                    allowFullScreen 
                    title="video"/>
            </div>
        </Fade>
    );
    
    const preview = (
        <Fade in={true}>
            <Paper elevation={0} className="drag-n-drop p-5" variant="outlined">
                <Paper elevation={3} className="icon-size rounded-circle mx-auto">
                    <YouTubeIcon className="m-icon"/>
                </Paper>
                <Typography component="div" className="mt-1 text-center">
                    <Box
                        fontSize="body2.fontSize"
                        fontWeight="fontWeightLight" 
                        m={1}>
                        Video shoud be from YouTube.
                    </Box>
                </Typography>
            </Paper>
        </Fade>        
    );
    
    return(
        <div className={className}>
            <ComponentTitle title={'YouTybe link'}/>
            <LineInput
                size="small"
                value={value}
                onChange={(event) => getVideoId(event.target.value)}
                className="w-100 mb-2 mt-0"
                placeholder="https://www.youtube.com/watch?v=I1uMutuYReM"
                variant="outlined"
            />
            <Typography component="div" className="row mt-1 text-danger">
                <Grow in={error}>
                    <Box
                        fontSize="body2.fontSize"
                        fontWeight="fontWeightRegular" >
                        Wrong link.
                    </Box>
                </Grow>
            </Typography>
            {videoUrl !== '' ? video : preview}
            <VideoLearnMore/>
        </div>
    )
}

VideoCol.propTypes = {
    videoUrl: PropTypes.string,
    onChange: PropTypes.func,
    className: PropTypes.string
};
