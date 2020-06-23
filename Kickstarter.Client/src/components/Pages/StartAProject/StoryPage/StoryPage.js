import React from "react";
import PageTitle from '../PageTitle';
import ColSubscribe from '../ColSubscribe';
import {Divider} from "@material-ui/core";
import {updateEditor, updateRisks} from "../../../../modules/actions/UpdateStory";
import {connect} from "react-redux";
import RisksCol from "./RisksCol";
import PropTypes from "prop-types";
import {CustomButton} from "../../../Decorators/Buttons";
import '../../../../styles/InputStyles.css';
import { DraftEditor } from "../../../DraftEditor/DraftEditor";
import Button from '@material-ui/core/Button';
import { convertToRaw } from 'draft-js'

const {storyTitle, storyDescription} = {
    storyTitle: 'Project description',
    storyDescription: 'Describe what you\'re raising funds to do, why you care about it, how you plan to make it happen, and who you are. Your description should tell backers everything they need to know. If possible, include images to show them what your project is all about and what rewards look like.\n'
};

const {risksTitle, risksDescription} = {
    risksTitle: 'Risks and challenges',
    risksDescription: 'Be honest about the potential risks and challenges of this project and how you plan to overcome them to complete it.'
};

const {pageTitle, pageDescription} = {
    pageTitle: 'Introduce your project',
    pageDescription: 'Tell people why they should be excited about your project. Get specific but be clear and be brief.'
};

function StoryPage(props) {
    
    const {updateEditor, updateRisks, editorState, risks, className} = props;
    
    const onEditorChange = newState => {
        updateEditor(newState);
    };
    
    const onRisksChange = newRisks => {
        updateRisks(newRisks);
    };
    
    const click = () => {
        const contentState = editorState.getCurrentContent();
        let res = convertToRaw(contentState); //JSON.stringify(convertToRaw(contentState));
        
        for (const [key, value] of Object.entries(res.entityMap)) {
            if (value.type === 'image'){
                value.data.filename = value.data.filename.name
                value.data.url = ''
            }
        }

        console.log(JSON.stringify(res));
    }

    return(
        <div className={className}>
            <PageTitle title={pageTitle} subtitle={pageDescription}/>
            <div className="row w-100 my-5 mx-auto">
                <ColSubscribe
                    title={storyTitle}
                    description={[storyDescription]}/>
            </div>

            <Button variant="contained" color="primary" onClick={click}>
                Primary
            </Button>

            <DraftEditor
                onChange={(editorState) => onEditorChange(editorState)}
                editorState={editorState}
                readOnly={false}
                className="mb-4 borders mx-4"
            />

            <Divider className="mt-4"/>
            <div className="row w-100 my-5 mx-auto">
                <ColSubscribe
                    title={risksTitle}
                    description={[risksDescription]}
                />
                <RisksCol
                    className="col mt1"
                    onChange={newRisks => onRisksChange(newRisks)}
                    text={risks}
                />
            </div>
            <Divider className="mt-4"/>
            <div className="my-4 row">
                <CustomButton variant="contained" size="medium" color="primary" className="mx-auto">
                    Save
                </CustomButton>
            </div>
        </div>
    )
}

const mapStateToProps = state => {
    return state.StoryReducer;
};

const mapDispatchToProps = dispatch => {
    return{
        updateEditor: editorState => dispatch(updateEditor(editorState)),
        updateRisks: (risks) => dispatch(updateRisks(risks))
    }
};

export default connect(
    mapStateToProps,
    mapDispatchToProps
)(StoryPage)

StoryPage.propTypes = {
    className: PropTypes.object
};