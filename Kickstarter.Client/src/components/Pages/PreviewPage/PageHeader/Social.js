import BookmarkBorderIcon from "@material-ui/icons/BookmarkBorder";
import IconButton from "@material-ui/core/IconButton";
import FacebookIcon from "@material-ui/icons/Facebook";
import TwitterIcon from "@material-ui/icons/Twitter";
import MailIcon from "@material-ui/icons/Mail";
import ShareIcon from "@material-ui/icons/Share";
import React from "react";
import {WhiteButton} from "../../../Decorators/Buttons";

export default function Social() {
    return(
        <div className="row mx-auto my-3">
            <WhiteButton
                variant="contained"
                color="secondary"
                className="mr-auto my-auto"
                size="medium"
                startIcon={<BookmarkBorderIcon/>}>
                Remember me
            </WhiteButton>
            <div className="row">
                <IconButton>
                    <FacebookIcon className="facebook"/>
                </IconButton>
                <IconButton>
                    <TwitterIcon className="twitter"/>
                </IconButton>
                <IconButton>
                    <MailIcon />
                </IconButton>
                <IconButton>
                    <ShareIcon />
                </IconButton>
            </div>
        </div>
    )
}