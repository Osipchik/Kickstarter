import React from "react";
import Button from "@material-ui/core/Button";
import { withStyles } from '@material-ui/core/styles';

export const CustomButton = withStyles(theme => ({
    root: {
        textTransform: 'none',
        color: theme.palette.getContrastText(theme.palette.green.main),
        backgroundColor: theme.palette.green.main,
        '&:hover': {
            color: theme.palette.getContrastText(theme.palette.green.dark),
            backgroundColor: theme.palette.green.dark,
        },
        selected: {},
    },
}))(props => <Button {...props} />);

export const WhiteButton = withStyles(theme => ({
    root: {
        border: '1px solid',
        textTransform: 'none',
        backgroundColor: theme.palette.textInput.light,
        borderColor: theme.palette.textInput.dark,
        '&:hover': {
            backgroundColor: theme.palette.textInput.light,
        },
        '&:active': {
            backgroundColor: theme.palette.textInput.light,
        },
        selected: {},
    },
}))(props => <Button {...props}/>);