import CardMedia from "@material-ui/core/CardMedia";
import React from "react";
import makeStyles from "@material-ui/core/styles/makeStyles";
import Card from "@material-ui/core/Card";

const useStyles = makeStyles(theme => ({
    card: {
        maxWidth: 345,
        borderRadius: 0,
    },
    media: {
        height: 0,
        paddingTop: '56.25%', // 16:9
    }
}));

export const CardMedia43 = (url) => {
    const classes = useStyles();
    
    return (
        <Card className={classes.card}>
            <CardMedia
                className={classes.media}
                image="https://interactive-examples.mdn.mozilla.net/media/examples/grapefruit-slice-332-332.jpg"
                title="Paella dish"
            />
        </Card>
    )
}