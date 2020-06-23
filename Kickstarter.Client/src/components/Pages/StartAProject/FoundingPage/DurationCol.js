import Typography from "@material-ui/core/Typography";
import Box from "@material-ui/core/Box";
import React, {useState} from "react";
import {Divider, makeStyles} from "@material-ui/core";
import Grow from "@material-ui/core/Grow";
import {GetDatesDifference} from "../../../methods";
import {ComponentTitle} from "../ComponentTitle";
import DateFnsLocalizationExample from '../../../Decorators/DateTimePicker';

const useStyles = makeStyles(theme => ({
    root: {
        border: `1px solid #BDBDBD`,
        overflow: 'hidden',
        borderRadius: 4,
        backgroundColor: theme.palette.textInput.main,
        transition: theme.transitions.create(['border-color']),
        '&:hover': {
            backgroundColor: 'inherit',
            borderColor: theme.palette.primary.light
        },
    }
}));

export default function DurationCol(props) {
    const {endDate, onEndDateChange} = props;
    
    const [dateError, setDateError] = useState(false);
    
    const handleDateChange = (date) => {
        if(GetDatesDifference(date) > 0){
            onEndDateChange(date);
            setDateError(false)
        }
        else {
            setDateError(true)
        }
    };
      
    const DateError = (
        <Typography component="div" className="row mb-2 text-danger">
            <Grow in={dateError}>
                <Box
                    className="mr-auto"
                    fontSize="body2.fontSize"
                    fontWeight="fontWeightRegular" >
                    The company must exist for at least one day.
                </Box>
            </Grow>
        </Typography>
    );
    
    //className={classes.root}

    const classes = useStyles();
    return(
        <div >
            <Divider className="w-100"/>
            <div className="mt-2 px-3">
                <ComponentTitle title="Company end date"/>
                <DateFnsLocalizationExample value={endDate} onChange={(date) => onEndDateChange(date)}/>
                {DateError}
            </div>
        </div>
    )
}