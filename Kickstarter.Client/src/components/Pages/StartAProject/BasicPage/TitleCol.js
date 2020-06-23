import React, {useState} from "react";
import Typography from "@material-ui/core/Typography";
import Box from "@material-ui/core/Box";
import {LineInput} from "../../../Decorators/TextInput/TextFields";
import Grow from "@material-ui/core/Grow";
import PropTypes from 'prop-types';
import {ComponentTitle} from "../ComponentTitle";

export default function TitleCol(props) {
    const {onChange, className, maxLen, title} = props;
    
    const [classes, setClasses] = useState(title.length > props.maxLen ? 'text-danger' : '');
    const [lenVisible, setLenVisible] = useState(title.length > 0);
    
    const onTitleChange = (title) => {
        onChange(title);
        setClasses(title.length > maxLen ? 'text-danger' : '');
    };
    const showTitleLength = () => setLenVisible(true);
    
    return(
        <div className={className}>
            <ComponentTitle title={'Title'}/>
            <LineInput
                onClick={showTitleLength}
                value={title}
                onChange={(event) => onTitleChange(event.target.value)}
                size="small"
                className="w-100"
                placeholder="Oath: Chronicles of Empire and Exile"
                variant="outlined"/>
            <Typography component="div" className={`row mt-1 ${classes}`}>
                <Grow in={classes.length > 0}>
                    <Box
                        className="mr-auto"
                        fontSize="body2.fontSize"
                        fontWeight="fontWeightLight" >
                        {`Title should not have more than ${maxLen} characters`}
                    </Box>
                </Grow>
                <Grow in={lenVisible}>
                    <Box
                        fontSize="body2.fontSize"
                        fontWeight="fontWeightLight" >
                        {title.length}/{maxLen}
                    </Box>
                </Grow>
            </Typography>
        </div>
    )
}

TitleCol.propTypes = {
    title: PropTypes.string,
    onChange: PropTypes.func,
    maxLen: PropTypes.number,
    className: PropTypes.object
};
