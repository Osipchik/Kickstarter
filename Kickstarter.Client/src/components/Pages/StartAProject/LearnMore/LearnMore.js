import React from "react";
import EmojiObjectsOutlinedIcon from "@material-ui/icons/EmojiObjectsOutlined";
import Box from "@material-ui/core/Box";
import Typography from "@material-ui/core/Typography";
import PropTypes from 'prop-types';

export function VideoLearnMore(className) {
    return(
        <Typography component="div" className={`row mt-1 text-success text-wrap ${className}`}>
            <EmojiObjectsOutlinedIcon fontSize="small" className="m-0 p-0"/>
            <Box
                className="mr-auto"
                fontSize="body2.fontSize"
                fontWeight="fontWeightRegular" >
                80% of successful projects have a video. Make a great one, regardless of your budget. Learn more...
            </Box>
        </Typography>
    )
}

export function DescriptionLearnMore(className) {
    return(
        <Typography component="div" className={`row mt-1 text-success text-wrap ${className}`}>
            <EmojiObjectsOutlinedIcon fontSize="small"/>
            <Box
                className="mr-auto"
                fontSize="body2.fontSize"
                fontWeight="fontWeightRegular" >
                Give backers the best first impression of your project with great titles. Learn more...
            </Box>
        </Typography>
    )
}

export function DurationLearnMore(className) {
    return(
        <Typography component="div" className={`row mt-1 text-success text-wrap ${className}`}>
            <EmojiObjectsOutlinedIcon fontSize="small"/>
            <Box
                className="mr-auto"
                fontSize="body2.fontSize"
                fontWeight="fontWeightRegular" >
                Campaigns that last 30 days or less are more likely to be successful. Learn more...
            </Box>
        </Typography>
    )
}

export function LearnMore(props) {
    const {className, message, link} = props;
    
    return (
        <Typography component="div" className={`row mt-1 text-success text-wrap ${className}`}>
            <EmojiObjectsOutlinedIcon fontSize="small"/>
            <Box
                className="mr-auto"
                fontSize="body2.fontSize"
                fontWeight="fontWeightRegular" >
                {message}
            </Box>
        </Typography>
    )
}

LearnMore.propTypes = {
    className: PropTypes.string,
    message: PropTypes.string,
    link: PropTypes.string
};