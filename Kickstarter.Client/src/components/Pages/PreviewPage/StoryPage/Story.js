import React from "react";
import { DraftEditor } from "../../../DraftEditor/DraftEditor";

export default class Story extends React.Component{

    constructor(props) {
        super(props);
        
        this.state = {
            story: props.story,
            risks: props.risks
        };
    }

    onChange(state){
        
    }
    
    render() {
        let editorState = this.state.story;
        
        return (
            <div className="col m-0 p-0">
                <div>
                    <DraftEditor
                        onChange={(state) => this.onChange(state)}
                        editorState={editorState}
                        readOnly={true}
                        className="mb-4"
                    />
                </div>
                <div>
                    <br/>
                    <hr/>
                    <br/>
                    <p className="title-smaller">Risks and challenges</p>
                    <p >{this.state.risks}</p>
                </div>
            </div>
        )
    }
}