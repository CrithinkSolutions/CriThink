import React, { Component } from "react";
import { WaveUpDown, WaveDown } from "./../Layout";
import { Menu, Segment, Label, Popup } from "semantic-ui-react";
import { ReactComponent as Logo } from './../../svg/logoround.svg';
import "./../../custom.css";

class MenuRender extends Component {
	render() {
		return (
			<Segment color={this.props.color} textAlign="left" className="noborderafter">
				<b>{this.props.header}</b>
				{this.props.children}
			</Segment>
		);
	}
}


class AnalysisNews extends Component {
	render() {
		return(
			<Segment basic>
				<Segment style={{width:'95%'}}>{this.props.text}</Segment>
				<Label basic attached='top right'>
					<Popup content={this.props.logoContent} trigger={<Logo />} />
				</Label>
			</Segment>
		)
	}
}


export class AnalysisArea extends Component {
	state = { activeItem: "OVERVIEW" };

	handleItemClick = (e, { name }) => this.setState({ activeItem: name });

	switchRender() {
		switch (this.state.activeItem) {
			case "OVERVIEW":
				return (
					<MenuRender color="red" header="DOMAIN">
						<Segment basic>
							Lorem ipsum dolor sit amet, consectetur adipiscing
							elit, sed do eiusmod tempor incididunt ut labore et
							dolore magna aliqua. Ut enim ad minim veniam, quis
							nostrud exercitation ullamco laboris nisi ut aliquip
							ex ea commodo consequat. Duis aute irure dolor in
							reprehenderit in voluptate velit esse cillum dolore
							eu fugiat nulla pariatur. Excepteur sint occaecat
							cupidatat non proident, sunt in culpa qui officia
							deserunt mollit anim id est laborum. Sed ut
							perspiciatis unde omnis iste natus error sit
							voluptatem accusantium doloremque laudantium, totam
							rem aperiam, eaque ipsa quae ab illo inventore
							veritatis et quasi architecto.
						</Segment>
					</MenuRender>
				);
			case "KEY WORDS":
				return (
					<MenuRender color="grey" header="Section under construction...">
					</MenuRender>
				);
			case "ANALYSIS":
				return (
					<MenuRender>
						<AnalysisNews 
						text="Lorem ipsum dolor sit amet, consectetur adipiscing
							elit, sed do eiusmod tempor incididunt ut labore et
							dolore magna aliqua. Ut enim ad minim veniam, quis
							nostrud exercitation ullamco laboris nisi ut aliquip
							ex ea commodo consequat. Duis aute irure dolor in
							reprehenderit in voluptate velit esse cillum dolore
							eu fugiat nulla pariatur. Excepteur sint occaecat
							cupidatat non proident, sunt in culpa qui officia
							deserunt mollit anim id est laborum. Sed ut
							perspiciatis unde omnis iste natus error sit
							voluptatem accusantium doloremque laudantium, totam
							rem aperiam, eaque ipsa quae ab illo inventore
							veritatis et quasi architecto." 
							logoContent='H for Header' 
						/>
					</MenuRender>
				);
			default:
				return null;
		}
	}

	render() {
		const { activeItem } = this.state;
		return (
			<div>
				<WaveDown namePage="Check Your News">
					<Menu pointing secondary size="huge">
						<Menu.Item
							name="OVERVIEW"
							active={activeItem === "OVERVIEW"}
							onClick={this.handleItemClick}
						/>
						<Menu.Item
							name="KEY WORDS"
							active={activeItem === "KEY WORDS"}
							onClick={this.handleItemClick}
						/>
						<Menu.Item
							name="ANALYSIS"
							active={activeItem === "ANALYSIS"}
							onClick={this.handleItemClick}
						/>
					</Menu>
					{this.switchRender()}
				</WaveDown>
			</div>
		);
	}
}
