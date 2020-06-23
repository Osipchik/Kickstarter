import React from 'react';
import TextField from '@material-ui/core/TextField';
import {useStylesReddit} from "./InputStyles";
import {fade, withStyles} from "@material-ui/core/styles";
import InputBase from '@material-ui/core/InputBase';

export function RedditTextField(props) {
    const classes = useStylesReddit();
    return <TextField InputProps={{ classes, disableUnderline: true }} {...props} variant="filled" />;
}

export const LineInput = withStyles(theme => ({
    input: {
        borderRadius: 4,
        backgroundColor: theme.palette.textInput.main,
        position: 'relative',
        border: '1px solid #ced4da',
        padding: '10px 12px',
        transition: theme.transitions.create(['border-color', 'box-shadow', 'background-color']),
        '&:hover': {
            backgroundColor: theme.palette.textInput.light,
            borderColor: theme.palette.primary.light
        },
        '&:focus': {
            borderRadius: 4,
            backgroundColor: theme.palette.textInput.light,
            boxShadow: `${fade(theme.palette.primary.light, 0.25)} 0 0 0 0.2rem`,
            borderColor: theme.palette.primary.main,
        },
    },
}))(InputBase);