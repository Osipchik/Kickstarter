import {fade, makeStyles} from "@material-ui/core/styles";

export const useStylesReddit = makeStyles(theme => ({
    root: {
        border: `1px solid #BDBDBD`,
        overflow: 'hidden',
        borderRadius: 4,
        backgroundColor: theme.palette.textInput.main,
        transition: theme.transitions.create(['border-color', 'box-shadow', 'background-color']),
        '&:hover': {
            backgroundColor: 'inherit',
            borderColor: theme.palette.primary.light
        },
        '&$focused': {
            backgroundColor: 'inherit',
            boxShadow: `${fade(theme.palette.primary.light, 0.25)} 0 0 0 4px`,
            borderColor: theme.palette.primary.main
        },
    },
    focused: {},
}));
