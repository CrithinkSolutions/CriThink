import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { WaveDown } from './../Layout';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { getDemoNews, getDemoNewsSelected } from '../../actions/demo';
import {
    Dropdown,
    Segment,
    Icon,
    Button,
    Popup,
    Loader,
} from 'semantic-ui-react';

class ChooseArea extends Component {
    componentDidMount () {
        this.props.getDemoNews();
    }

    handleSelection = (event, data) => {
        const link = data.value;
        const { text } = data.options.find((o) => o.value === link);
        this.props.getDemoNewsSelected(link, text);
    };

    render () {
        return (
            <div>
                <WaveDown namePage="Check Your News">
                    <Segment basic size="massive">
                        <span className="light">CHOOSE YOUR SOURCE</span>
                        <Popup
                            content="Add users to your feed"
                            trigger={
                                <Icon
                                    name="info circle"
                                    style={{ color: '#FF9600', margin: '1rem' }}
                                />
                            }
                        />
                    </Segment>
                    {!!this.props.demoLinks ? (
                        <Dropdown
                            className="light dropdowncss"
                            fluid
                            selection
                            options={this.props.demoLinks}
                            onChange={this.handleSelection}
                        />
                    ) : (
                        <Loader active />
                    )}
                    <Segment basic>
                        <Link to="/4">
                            <Button className="btnorange">
                                <span className="regular">SEE THE RESULT</span>
                            </Button>
                        </Link>
                    </Segment>
                </WaveDown>
            </div>
        );
    }
}

function mapStateToProps (state) {
    return {
        demoLinks: state.demo.demoNews.map(news => ({
            value: news.url,
            text: news.title,
        })),
    };
}

function mapDispatchToProps (dispatch) {
    return bindActionCreators({
        getDemoNews,
        getDemoNewsSelected,
    }, dispatch);
}

export default connect(mapStateToProps, mapDispatchToProps)(ChooseArea);
