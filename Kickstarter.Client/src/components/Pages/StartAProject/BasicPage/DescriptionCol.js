import React, {useState} from "react";
import Typography from "@material-ui/core/Typography";
import Box from "@material-ui/core/Box";
import {LineInput} from "../../../Decorators/TextInput/TextFields";
import Grow from "@material-ui/core/Grow";
import PropTypes from "prop-types";
import {DescriptionLearnMore} from "../LearnMore/LearnMore";
import {ComponentTitle} from "../ComponentTitle";

export default function DescriptionCol(props) {
    const {onChange, description, maxLen, className} = props;
    
    const [classes, setClasses] = useState(description.length > props.maxLen ? 'text-danger' : '');
    const [lenVisible, setLenVisible] = useState(description.length > 0);

    const onTitleChange = (description) => {
        onChange(description);
        setClasses(description.length > maxLen ? 'text-danger' : '');
    };
    const showTitleLength = () => setLenVisible(true);

    return(
        <div className={className}>
            <ComponentTitle title={'Description'}/>
            <LineInput
                onClick={showTitleLength}
                value={description}
                multiline={true}
                onChange={(event) => onTitleChange(event.target.value)}
                size="small"
                className="w-100 pt-0"
                placeholder="Designed by Cole Wehrle, an innovative strategy game for 1 - 6 players about remembering the history that would've been forgotten."
                variant="outlined"/>
            <Typography component="div" className={`row mt-1 ${classes}`}>
                <Grow in={classes.length > 0}>
                    <Box
                        className="mr-auto"
                        fontSize="body2.fontSize"
                        fontWeight="fontWeightLight" >
                        {`Description should not have more than ${maxLen} characters`}
                    </Box>
                </Grow>
                <Grow in={lenVisible}>
                    <Box
                        fontSize="body2.fontSize"
                        fontWeight="fontWeightLight" >
                        {description.length}/{maxLen}
                    </Box>
                </Grow>
            </Typography>
            <DescriptionLearnMore/>
        </div>
    )
}

DescriptionCol.propTypes = {
    description: PropTypes.string,
    onChange: PropTypes.func,
    maxLen: PropTypes.number,
    className: PropTypes.object
};