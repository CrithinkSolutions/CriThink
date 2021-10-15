using System;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;
using CriThink.Client.Core.ViewModels.NewsChecker;
using CriThink.Client.Droid.Constants;
using Google.Android.Material.BottomSheet;
using MvvmCross.DroidX.Material;
using MvvmCross.DroidX.RecyclerView;
using MvvmCross.Platforms.Android.Presenters.Attributes;

namespace CriThink.Client.Droid.Views.NewsChecker.Dialogs
{
    [MvxDialogFragmentPresentation]
    [Register(nameof(QuestionAboutNewsView))]
    public class QuestionAboutNewsView : MvxBottomSheetDialogFragment<QuestionNewsViewModel>
    {
        public QuestionAboutNewsView()
        {
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = View.Inflate(Context, Resource.Layout.editprofile_view, null);
            var listQuestionNews = view.FindViewById<MvxRecyclerView>(Resource.Id.recyclerQuestions);
            var layoutManager = new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false);


            using (var set = CreateBindingSet())
            {

            }

         
            var behavior = (Dialog as BottomSheetDialog).Behavior;
            behavior.HalfExpandedRatio = BottomSheetConstants.HalfExpandRatio;
            behavior.State = BottomSheetBehavior.StateHalfExpanded;
            behavior.PeekHeight = BottomSheetConstants.PeekHeight;
            behavior.GestureInsetBottomIgnored = false;
            behavior.FitToContents = false;
            behavior.ExpandedOffset = 0;
            behavior.Hideable = false;
            behavior.SkipCollapsed = false;
            behavior.AddBottomSheetCallback(new BottomSheetToolbarToggleCallback(this));
          
            return view;
        }
    }
    public class BottomSheetToolbarToggleCallback : BottomSheetBehavior.BottomSheetCallback
    {
        public BottomSheetToolbarToggleCallback(BottomSheetDialogFragment bottomSheetDialogFragment)
        {
            this._bottomSheetDialogFragment = bottomSheetDialogFragment ?? throw new System.ArgumentNullException(nameof(bottomSheetDialogFragment));
        }
        public override void OnSlide(View bottomSheet, float slideOffset)
        {
        }
        public override void OnStateChanged(View bottomSheet, int newState)
        {
            switch (newState)
            {
                case BottomSheetBehavior.StateCollapsed:
                    ShowToolbar(bottomSheet, ViewStates.Gone);
                    break;

                case BottomSheetBehavior.StateHalfExpanded:
                    ShowToolbar(bottomSheet, ViewStates.Gone);
                    break;
                case BottomSheetBehavior.StateExpanded:
                    ShowToolbar(bottomSheet, ViewStates.Visible);
                    break;
                case BottomSheetBehavior.StateHidden:
                    _bottomSheetDialogFragment.Dismiss();
                    break;
            }
        }
        private void ShowToolbar(View bottomSheet, ViewStates viewState)
        {
            var toolbar = bottomSheet.FindViewById<Toolbar>(Resource.Id.toolbar);
            if (toolbar != null)
            {
                toolbar.Visibility = viewState;
            }
        }
        private readonly BottomSheetDialogFragment _bottomSheetDialogFragment;
    }
}
/*
 * ,,.....,,'..'..',,'.',,..'''',,'.',,,',,'',;:::;;;;:;;;;;;;::;;;;;;;:::::::;;::::::::::::::::::::::::::::;;;;;;;;;:::;.';;:,..;::::::::::::::::;:::::::::::;;;::::::::::::::::::::::;;::oxxl'.........':
cc,,,,,;cc;,,,,:c:;,;::,;::;::::;::;;;::;;coooooooooooooddoodddooooooodoooooooooooooooooooooodddddddooooooooooooooooc;:lool;,:odoooooooooooddoooooooooooooooooooooooooddooddddddooooooox0NNk;........':d
cc;,,,,,;cc:;,,:c:,,;::,;::;::::;:::;:c:;;loolooooooooooooooooddooooooooooooooooooooooooooooddddoddooooooooooooooooc;:clooc,;oooooooooooooooooooooooooooooooooooooooododooddooooooooddox0XXx,........'cd
cc;,;;;;,,:l:,,:c:,,;::,;::;::::;:::::c:;:loooooooooooooooooooodooooooooooooooooooooooooooooooooooooooooooooooooooc;:;;col,'codddoooddoooooooooooooooooooooooooooooooooooooooooooodddddkKNXx;'',,,,,,:ld
::,,,,;:::cl:;,;:;,,;::;::;;:::;;:cc::c::cooolloooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooool:;::;;loc,,coddooooooolllllooooooooolllllllllllllllllooooooooooooddxxxkOOOkxddxxxxxxxxx
,,,,,,;:ccc:;,,;;;,,,;:::;,;;;,,,;:;;;;;;cooollooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooooo:;;clc:loc''cooolloooollllllllllllllllllllcccllcllllllllollllllllllooooooodddddooddddddd
,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,;cllllllllooooooollllllloooooooolooooooooooooooooloolloooooooooolooolll:;:cdxc:odl,'clllllllllllllccllccccccccclcccccccccccccccccccccccllllllllllllllllllllllllo
,;,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,;;;;;:::cllllllllllllllllllllllllllllllllolllllllllllllllllllllllllllllllllll:;::lkkccodo;':lllllllllcccccccccccccccccccc::::::cccccccccccccccccllllcccllllllllllllllll
,,,,,,,;,,,,,,;;;;;;;;;:::::::::ccccccclllllllllllllllcllllllllllllllllllllllllllllllllllllllllllllllllllllllc;;:lx0kl:loo:',clcclllccccccccccccccccccc:::::;;:::::::::cccccccccccccccccccccclllllllllll
,;;;;:::::cccccccccccllccclllccccccccccccccccclllllllllllcclllccllllclllllllllllllllllccllllllllllllllllllllc;:ccd00kl:lool,':llcccccccccccccccccc:::::::::;;;;;;;;;;;;;;::::::::::::c::cccccclccccccccc
:llllllllcccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccllcccccccccccllclllllllllllllllllllc;;cclk00kl:lool;';cccccccc:::c:::::::::::::::;;;;;;;;,,,,,,,,;;;;::::::::::::::::ccccccc::;;
:lllllccccccccccccccccccccccccccc::::cccccccccccccccccccccccccccccccccccccccccccccccccccccllcclllcllllccclc;,:ccdO00Ooclooo:',ccc:ccc::::::;:;;;;;;;;;;;;,,,,,,,;;;,,,,,,,,,;;;;;;;;;;;;;;;;;::;;;;,,;;:
:ccllcccccccccccccccccc::::ccccc::::ccccccccccccccccccccccc:::::::::cc:ccccccccccccccccccccccccccccccccccc:,;c:lk000Oo:looo:.':c::::;;;;;;;,;,,,,,,,,,,,,,,,,,,,;;;;;,,,,,,,,,,,,,,,,,,,,,,,,,,,,;:ccloo
:cccccccccccccccccc::::::::::cc:::::::::::::::::::::::::::::::::::::::ccc:::ccc:::ccccccccccccccccc::cccc:;,:c:oO000Oocloooc'.;::;:;;,,;;;,,,,,,,,,,,,,,,,,,,,,;;::;;,,,,,,,,,,,,,,,,,,'''',;:cllooooolc
:ccccccccc:::::::::;;:::::::::::::::::::::::::::::::::::;::::::::::::::c:::::::::::c:::ccc::::::::::::::::,,:ccd00O0Odcloool'.,;;;;;;;,,,,,,,,,,,,,,,,,,,,,,,,,,;;;;,,,,,,,,,,,,,,,'''',;:cllllc;;:lol;.
:ccc:::::::;;:;;::;;;;;;;;;;;::;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;::;;;:::::::::;;::::::::::::::::::::::::::;',cccx0000Odcloddl'.,;;;;,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,',,,,,,'''',:cloollcc:;;cloc,..
;:::;;;;;;;;;;;,,;;,,;,,;;;,;;,,,,;;,;;;,,;;;;;;;;;;,,;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;,',cclk0OO0Oocloddl'.';;;;,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,''',,,,,,,,''..';:llllllc:col::looc,..,
,;;;;;,;,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,;;,,;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;;,.;c:lk0OO0klcloool'.,;,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,'',,,,,,''..';:lloloolcccoxo:cloo:'.'',
,;;;,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,;,,;;;;,;;;;;;;;;;;;;;,;;;;,,,,,;;'.;l:lk0OO0klcooodc'.,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,'..,:clllllolccldkxl:cool;..''''
,;;;,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,;;;,,,,,,;;,,,,,,,,'.;lclO0OOOxccoooo:..,,,,,,,,,,,,,,,,,,,,,,,,,,,,,',,,,,',',,'..,cllllooolcclxOOd::looc,..','''
,,,,,,;,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,;;;,,,,,,;;;,,,,,,,'.;occx00OOdccoooo;..,;,,,,;;,,,,,,,,,,,,,,',,,,,''',,,,'',''.,clllloolc:coxOOko:clol;..'''''''
,,,,;;,,,,,,,',,,,,,,,,,,''',,,,,,,,,,,',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,;;;,,,,,;;;;,,,,,,,'.;ol:d000kl:lddoc'.,;;;;,,;;,,,,,,,,,,,,,,,,,,,,,,,,,,,,''.,:loooooolccokOOOxlccoo:'..''''''''
,;,,,;,,,,,,,,',,,,',,,,,'''''''',,''''',,,',,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,,;;;,,,,,,,,,,,,,,,,'.;ol:oO0Od:;cccc,.',,,,,',,,,,,,,,,,,,'',,,,,,,,,,,,,'''.':looloollccokOOOkoccloc,..',''''''''
:lccccccccccc:::::::::::;;;;;;;;,;,,,,,,,,,,,,,,,,,,,,,,,,,'',,,,,,,,,,,,,,,,,,,,,,,,,;;;;,,,,,,,,,,,,,,,,',loclkxollloooooooollc::;;,,,,,,',,,,,,,,,,,,,,,,,,''..,coooloolc:oxOOOOdccllc,..'','''''''''
lddooooooooooooooooooooollllllllllllcccccccccccc:::::::;;;;;;,,;,,,,,,,,,,,,,,,,,,,,;;,;;;,,,,,,,,,,,,,,,,.'colclclxxxkkkkkkkkkkxxxxddollc;,,,,'',,,,,,,,,,,,,'.,:looooolcclxOOOOxl:clc;..',,,,'''''''''
loodddoooooooooooooooooooooooooooooooooooooooooooooooolllllllllllccccccccc:::::::::::::;;;;;;;;;;;;;;;;;;;'.:ooc,.';,,,;;;::clooodxxkkkkkkxxdolc:;,,'',,,,,,,..;lllllool:cdO0OOkocclc;'.'',,,,''''''',''
coooooooooooooooooooooooooooooooooooooooooooooooooooooooodooooooooooodddoooooooooolllllllcccccccccccccccc:;';odl'              ...',;:clodxkkkkkxdolc;,'',,'.':lollool:cdkO0Okoccll:'.',,,,,,'''',''''''
lddddooooooooooooooooooooooooooooooodoooooooooooooooooooooodoooooooooooooooooooooooollclccccccclccccccccc:;,,cc;.                       ..',;cldxxkkxxo:'.'.,cooooooccoxO0Okdccll:,.',,,,,',,'',,,,,''''
lddddddooooooooddoooooooooooooooooodddddooooooooooooooooooooooooooooooooooooooooooooc:cooolloooooooooooooooolollc:;,''....                     ..,:ldxxl'..;coooooccokOOOxoc:cc;''',,,,,,,,'''',,,,,,,,,
ldddddddddoddodddddddddddddddooodddddddddddodddooooooodoooooooooddddddoddddddoooooddc;:ooooooooooollllllllllllllooooooooollc:;;,'....               .','.':oooool:cdO0Oxlc:cc;,,;:;;;;;;,,,,,,,,,,,,,,,,
lddddddddddddddddddddddddddddodddddddodddddoddooddddoddddddddddddddddddddddddoooddddoc:cldkkkkkkkkkkxxxxxxxddddodooooollllllloooooollcc:,''..          .'coooolccdkkxolcc::;:clolllllllcccccccccc:cc::::
ldddddddddddddddddddddddddddddddddddddddddddoddddddddddddddddddddddddddddddddddddddddxdlcccodxkkkkkxxxxxxkkkkxxxxxkkkkkxxxdddooolllllooooooolc:;,...   'looooc;:odolccc:::clodoooooooooooooooooooooooooo
odddddddddxxdddxddddxxxddxxxxdxxxddxxddddxxdddxxdddddddddddxxxddxxxxxxxdddddxxxxxxddxxxdddollccloxkkkkxxxxxkxxdoolclloodddxxxxkkkxxxxdddooolllllollcc:::cccllc:clllcc:::lodddooooooooooooodddddddddodddd
oxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxdddxxdollllloooodxxkkkxdlcc::cc::cccllooodddxxxkkxxxxxddoollllllccccccccc::ccodddddddddddoodddoooddddddddddddddx
dxxxxxxxxxxxxxkkkkxxxxkkxxxxxxkkkkxxxxxxxxxxkxxxxxxxxxxxxxxxxxxxxkkkkkkxxxxxxkkxxxxxxdddddddddddxxdoolccccccllccclccdkOdclc;cllccccclllllloodxxxxxxdddddooollcc:;;:clooddddddddxddddddddddddddddddddddxx
dkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkOkkkkkkkkkkkkkkkkkkkkkOOkkOkkkxkkkkkkkkkkkxxxxxxxxolclllooxkkdocc::cc:cc,'lxlll:cooooooolc:::::cccccoddxxxxkxxxxxdoolcc:cccclldxxxxxxxdxxxxxxxxxxxxxxkkkkxxx
xOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO00OOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO0OOkOOOOOOOOOkkkxxxxxkkdl:oOKKKKK00KKXXXKko;,. .dkc:oooolccoolccclol:::;,;lodxxxxkxxxkkkkxxdoolllcc:cloxkkkkxxkkkkkkkkkkkkkOOOOOOk
k0000O000000000000000000000000000000000OOOO000OOOOOOOO0O000OO00000000000000000OO000OO00OOOxdoodk0KKKKKXKKK000KNNN0o',oko:;ldol:cloolllloxdccc;.;oxxxxxxxxxxxxxxxxxxxxxdolll:;lxOOOOOkkkOOOOOOOOOOOOOOOOO
OK000000KKK0KKK000000KKK0KK00000000000000000000000000000KKKKKKKKKXXXXXKKKKKKKKKKKK00KKKK0kkOKNNWNNXKKK00000OOkkk0KXOollllc;;;,:cccodddOXXxcldc';dxxxxxxxxxxxxxxxkkkkxxkkkxdl::dO0000OOOOOO00000000000000
0XXKKKXXXXXKKXXXXXXXXXXXXXXKKKKKKKKKKXKKKKKKXXXXXKKXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXKKKKKOdldOKKKK00KKKK0000Okkkkdddx0klccccc:;,;::oxc'.cOxllddc,;looooddxxddxddxxxkkxxdddddolcoOKKKKKK0KKKKKKKKKKKKKKKKKX
KNNXXKKKXXXXXNNNNNNNXNNNNNNXNNXXNNNNNNNNNNNNXNNNNNNNNNNNNNNNNXXXNNNNNNNXXXXXXXXXNNXK0000kooOKKKXXXXXXXKKKKKKKKXXXXKK0dl:cccccc:cdl'  .;c:cool;,lkkxxxdddooooodddoooddddddxxk0KKXXXXXKKXXXKKXXXXXXXXXXXXK
0NNNNXXXXXNNNNNNNNNNNNNNNNNNNNNXXNNNNNNNNNNNNNNNNNNNXNNNNNNNNXXNNNNNNNNNNNNXXXXNNNNNXNNNKxkXWWWWWWWWWWWNXK000O0Oxx0NX0xc:cllodooxdoodxxkkxdo:,;xXNXXXKKKKKK0000000KKKKKKXXXXXXXXXXXKKKKXXXXXXXXXXXKKXXKK
KNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNXXNNNNNNNNNNNNNNNNXNNNNNNNNNNNNNXNNNNXXNNNNN0xkXNWWWWWWWWWNK000KKKKKOkkOOOkkk0KNNKkxxkkO0000000Okxoox0XXXXKXXXKKKKKXXXXXXXXXXXXKKXXXXXKKKKXXXXXXXXXXKKKKKKKK
KNNNNNNNNNXXNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNXNNNNNNXXXNNNNXXNNNNNNNNNXXNNNNXNNNNNNNNXXXNKkolokNWWWWWWWWWWWWWWWWWNXKXK0OOO00000OkxxxxxxkkkO00000kdlokOOO0000KKXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXKKKKXXKX
KNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNXXXXXXXXNNNNNNNNNNNNXXNNNNXXXNNNNNNNNNXXNKkdodOXWWWWWWWWWWWWWWWWN0kO0KXXXXXK0xdxOOkkkO0OOOOO0KXNN0ooO0000OO0KXXXXXXXXXXXXXXXXXXXXXXXXXKXXXXXXXXXXXXXXXXX
KNNNNNNNNNNNNNNNNNNNNNWNNNNNNNNNNNNNNNNNNNNNXNXXXXNNNNNNNNXXXXXNNNNXNNNXXNNNNNNNNNNNNNXXXXXXX0kxOXWWWWWWWWWWWWWWWNXK00OKNWWWWN0kkKKOkOO0KXX0OOOO0Ol;d0NNXXXXXXXXXXXXXXXXXXXXXXXXXXXXKXXXXXXXXXXXXXXXNNNX
KNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNXXXXNNNXXNXXNNNNNNNNNXNNXXNNNNXNNNNXNNNXNNXKxllx0XNWWWXXNWWWWWWWWWWWWWWWWWWWKxkX0k0K0OOKNWWNXKx:,:dKNXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
KNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNXXNNNNNNNNNNNNNNNNNXXNNNNNNNX0kdool::oxO0XN0xONWWWWWWWMWWWWWWWWWNXkx00kONWNK0O0XNWW0ookxxO0KXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
KNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNXXNNNNNNNNNNNNNNNNNNNNNNNNNNNNX0kolloddooOXXX0kO0OkOXNWWWMMWWWWWWWWX0OOO00OdoONWWNX0OkkkkoxXXKOOKXXXXNNXXXXXXNNXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
KNNNNNNNNNNNNNNNNNNNNWNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNXNNNNNNNNNNNNNNNNNNNNNNNNNNX0kdlloddxdooONWWWNX0kk00OO00KXXXNNWWWXOk0KNNXOxlclxKWWWNKdc;;:xXXXNNXXXXXXNXXNNXXXXNNXXXXXXXXXXXXXXXXXXXXNNXXXXXXXX
KNNNNXNNNNNNNNNNNNNNNWNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNX0kdlloddddxdolkNWWWWWWWXxoOXXXKKKKKXNWNKkkXWNKkoloddoco0NXOxk0Oo:lx0XNXXXNXXXXXXXXXXXNNNXXXXXNXXXXXXXXNNXXXXNNNXNNNNXX
0XXXXXXXNNNNNNNNNNNNNNNNXNNNNNNNNNNNNNNWWNNNNNNNNNNNNNNNNNNNNNNNNXNNNNNNNNNNNNX0xdllodddddddddloKWWWWWWWWNkdkOKWWWNXK000OO0NNKxooddddddo:cdddkKNWNKxllkKXXNNNNXNNNNNNNXXXXXXXXNNXXXXXXXXXXXXXXXXXXXXXXXN
OXXKKKKXXXXXNNNNXNNNNNNNXNNNNNNNNNNNNNNWWNNNNNNNNNNNNNNNNNNNXNNNNNXNNXNNNNNKOxollodddddddddddolkNWWWWWWWWNkk0kxOK0OOO0KXNNNKkoooddddddddc:d0KKKK0OOkxddOXNNNXXXXXXXNNXXXXNNNXXNXXXXXXXXXXXXXXXXXXXXXXXXX
kKKKKKKXXXKKXXXXXXXNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNNXNNNNNNNNX0xollodddddddddddddoclONWWWWWWNXOxxO0xdxOKNWWWWWXkoloooloddddodl;ckOOkOOOKKKXXXNNNNXXXXXXNNNXXNNNXXNXXXXXXXXXXXXXXXXXXXXXXXXXXX
xOOOOO0KKKKKKKKKKKXXXXXXXXXXXXNNXXXNNXNNNNNNNNNNNNNNNNNNNNNNNNXXXNNNNX0xollodddddddddddddollolo0WWWWWWKOk0XXKOk0XWWWWWWN0doooccccodddoodo:cONNXXXXNNXXXNNNNNXXXXNNNNNXXXXXXNNXXXXXXNNXXXXXXXXNXXXXXXXXXX
xkkkOkkOOO000000000KKKKKXXXXXXXXXXXNNXXXXXXXNNNNNNNNNNXNNXXXNNXXNNX0kdoloodddddddxdddollodkKXOx0WWWWWX00NWWKkOXWWWWWWWKxllodd:,:oddddoodl:cONNXXXXXXXNNNXXXXXXXXXNNNNXXXXXXNNNNNNXXNNNNXXXXXXNXXXXNNNNNN
dOOOkkkOOkOOOOOO0000KKKKKKKKKXXXKXXXXXXXKKKXXXXXXXXXXXXXXXXXXXXXKOdlloddddddddddddollldk0XNNXOdONWWWWWNWWWWXXNWWWWWWNOdlodddl;,cddddddddl:l0NXXXXXXXXXXNXXXNNNXNNNNXXXXXXXXXXXXXXXNNNNNNXXXXXXXNNNNNNNNN
xOOkkkkOOOOOOOOOOOOO0000000KKXXKKXXKKXXKKKXXXXXXXXXXXXXXXXXXXKOdoooddddddddddddollldO0XXXNNNX0xkXWWWWWWWWWWWWWWWWWNKkdoodddlclllodddddxdo:oKNXXXXXXXXNNXXXNNNXXXNNNNNNXXNNNNNNXXXNNXXXXNNXXXXNNNNXXXNNNN
xOOOOOOOOOOOOOOOOOOOO000000KKKKKKKKKK00KKKKXXKKKKXXXXXXXXXK0xollodddddddddddollodOKXXXXXXXXXXXkd0NWWWWWWMWWWWWWWNK0Okxddddlcxkocooooddxdl:dXNXXXXXXXXNNXXXXXXXXNXXNNNNNNNNNXXNXXNNNXXXXXXXXXXNNXXXNNXXXX
xOOOOOOOO0O00OO000000000KKKKKKKKKKKKKKKKKKKKXKKKKKKXXXXXKkdllodddddddddddollldOKXXXXXXXXXXXNX0xoxXWWWWWWWWWWWWXK00KKkddddllxK0olodddddddcckXXXXNNXXXXXXNNNNXXXXXXXXNNNXXXXXXXXXXNNXXXXXXXXXXXXXXXXNXXXNX
k00000000KK000000KKKKK0KKKKKKKK0KKKKKKKKKKKKKKKK0KKKKK0xolloddddddddddollldOKXXXXXXNNXXXXNNXOk0OkONWWWWWWWWNK0OKXNKkddddllxKNOoldddddddo:cOXXXXXXXXXXXXXNNNXXXXXNNNNNXXXXXXXXXXXXNNXXXXXXXXXXXXXXXXXNNNX
OK000000000000000KKKKKKKKKKKKK000KKKK00KKKKKKKKKKKK0Odllodddddddddddollok0XNNXXXXXNNXXXXXNKkkKNNOxKWWWWWWXK00KNWWXkddddlcdKXXklldddddddo:l0XXXXXXXXXXXXXXXXXXXXXXNNNXXXXXKXNXXXXXXXXXXXXXXXXXXXXNXXXXXXX
k0000OOOOO0K000KKKKKKKKKKKKKKKKKKKXKKKKKKKKK0KKK00Oxlcoddddddddddollox0XXXXXXXNNXXXNNNNNX0xkKWWWKkONWWXK00KNWWWWXOxdddocd0XXXxcldddddddl:oKXXXXXXXXXXXXXXNNXXXXXXXXXXXXXXXNNXXNNXXXXXXXXXXXXXXXXXNNNNXXN
OK0K000000KKKKKXKKXXXXXKKKKKKKXXXXXXXXXXXXKKKKKOkk0OdcldddddddooloxOKXXXXXXXNNNNXXXXNNNX0xkXWWWWXOxO0OO0XNWWWWWNOddxxocoOXXXKdcoddddddoc:xXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXNNXXXXXXXXXXXXXXXXXXXXXXN
OKKKKKKXKKXXK0KXXKXXXXXXXXXXXXXXX0kkkxkkOOkkxxkdxKWWXOxddooolclx0KXXNXXNXXXXNNNNNNXXXXX0xOXWWWWWWX0KKXNWWWWWWWN0xddddllkXXXNKocoddddddo:cOXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXNXXXXXXXXXXXXXXXXXXXXXXX
OXXXXXXXXXXXXKKXXNXXXXXXXNXK0OOkxx0KXK0O0KXXKkoodxOKNNNNXXK0dcxXNXXNNXXNXXXXNNNNXXXNNXOxONWWWWWWWWWWWWWWWWWWWWKkddddlcxKXXXXOoldddddddo:l0XXXXXXXXXXXXXXXXXXXXXXXNXXXNXXXXXXNXXXXXXXXXXXXXXXXXXXXXXXXXXX
0XXNNNXXNNNNNNNXNNNXNXXKOxdolllcckNWWWWWWWWNX0k0XKOxdxxkOOdloOKNNXXNNXXXXXNNNNXXXXNNX0xONWWWWWWWWWWWWWWWMWWWWXOxxddoco0XXXXXxcldddddddo:dKXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
0XNNNNXNNNNNNNNNNXXXKkdollloodol:cxOOOkkKNXOk0NWWWNOkkxxO0dlONNNXXXXNNXXXXNNNNNNXXNX0xONWWWWWWWWWWWWWWWWWWWWNOxxxdoclOXXXXXKdcododddddl:xXXXXXXXXXXXXXXXNNXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXNNXXKK
0NNNNNNNNNNNNNNXXNKOdllooooodolcclllllccdkkOXWWWWWKOOkkkOkdxKNNNXXNXXNXXXNNXNNNNNNX0xONWWWWWWWWWWWWWWWWWWWWWKxdxddlcdKXXXXX0dcodddddddlckXNXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXNNNNXXXXXXXXXXXXXXXXXXXNNXXK
0NNNNNNNNNNNNNNNNKxloooooooooooooddooooc:l0NWWWWWXOOOkkkkodKNNNNNXXNXXXXXXXNNNXXNX0xONWWWWWWWWWWWWWWWWWWWWWXOxxxxdclOXXXXXXOocoddddddoclOXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXNXXNNXXXXXXXXXNXXXXNXXXXNXXX
0NNNNNNNNNNNNNNNXklloooooooooooooddolllclONWWWWWX000OOkdox0NXXNNNNNNNXXXXXNNNNNNXOxONWWWWWWMMMMWWWWWWWWWWWN0xdxxdl:dKXXKXXXkcloddddddoco0XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXNX0k0NNXK00KXX
0NNNNNNNNNNNNNNNKxclolc:::cloddddolclodoxXWWWWWWNXK00OdlxKNXXNNNNNNNNXXXNNNXXXNN0xONWWWWWWWWMMMMWWWWWWWWWWXkddxxd:cOXXXXXXXxcldddddddlcxXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXNXXXXXXXXXXXXXXXKKXXXK00XXX
KNNNNNNNNNNNNNNNXOo:clloool::loolccoxxdoONWWWWWWWWXNNOccOXNXNNXXXNNNNNNNNNNNNNX0xONWWWWWWWWWWMMMWWWWWWWWWN0xddddo:o0XXXXXX0ocoddddddoccOXXXXXXXXXXXXXXXXXXXXXXXKXXXXXXXXXXXXNNXXXXXXXXXXXXXXXXXXNXXXXXXX
0NNNNNNNNNNNNNX0kdoododdxxxdc:ccldxxxxodKWWWWWWWWWWWKd;:kXNNXXXXXNNNNXXXXNNNNXOxONWMWWWWWWWWWMMWWWWWWWWWWXkdddddlcxXNXXXXXkllododdddo:o0XXXNXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXNNNNNXXK00000OOOOO
0NNNNNNNNNNXXXOddxxxxxdodxxxxxxxxxxxxdokXWWWWWWWWWWNkc:o0NNNXNNNNXNNNXXXNNNNXOdONWWWWWWWWWWWWWMWWWWWWWWWN0xddddoclONNXXXXKdclddoddddl:dKXXXXXXXXXXXXNXXXXXXXXXXXXXXXXNNNXXXXXXXXXXXXXNWWWWWWNK0Okkxxxxxx
KWWNNNNNNNNNXOdxkkxxdodxoodxxxxxxxxxxdd0NWWWWWWNNWWKoco0XNNXXNNNNNNNNNXXNNNXOdONWWWWWWWWWWWWWWWWWWWWWWWWXOxdddxlcdKXXXXNXOl:ldoooolocckXXXXXXXXXXXXXNXXXXXXXXXXXKKXXXXXXXXXXXXXXXXKkdxOO0XXNNKOkkkkkkkkk
KWWNNNNNNNNNXkdxkkdc;,:oxoldxxxxxxxxdokXWNNWWWK0XWXdlx0XNXXNNNNNNNNNNNXNNNXOxONWWWWWWWWWWWWWWWWWWWWWWWWWKkdddddcckXXKKXNXkld0XXXX0xl;cOXXXXXXXXXXXXXXXXXXXXXXKKKKKKKXXXXXXXXXXKKXXKkl:;;:lloxkkkkOOOOOOO
KWNNNNNXXXXNXkodkxdc;;;lxoldxxxxxdxdoxKWN0KNWXO0X0xx0XNNNNNNNNNNNNNNNXXXNN0xONWWWWWWWWWWWWWWWWWWWWWWWWWNOxddddoco0XXXXXN0xk0K0O0KXXx;cOXXXXXXXXXXXXXXXXXXXXXXXKXXXXXXXXXXXXXXXXKKXXKxl;,;::;cdOOO00K0Okk
KNNNNNNNNXXXN0doooc:;,cddlldxxxdooookKNNK0XX0xxkxxOXNNNNNNNNNNNNNNNNNXXNNKxkXWWWWMWWWWMMMMMMMWWWWWWWMWWKkdddxdlcxXNXXXXXklxOKKK0kdkkdox0XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXKKXXX0xc;:c:;:oOOOOKKOkkO
0NNNNNNNNNNNNNOl;,;:;;cddoodol:;:lx0NNKkxkkxddxk0XNNNNNNNNNNNNNNXXNNNNNNKkxXWWWWMMMMMMMMMMMMMWWWWWWWMWNOddddddcckXXXXXX0xkXNWWWWXO0NNKxdkXNXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXKKXXXK0d::::::cdkOO00kkkk
0NNNNNNNNNNNXko:;;::,,coddddxkxolokOOkxddxkOKXNNXNNXXXXXNNNNNNNXXNNNNNNXkx0WWWWWWMMMMMMMMMMMMWWWWWMMMWKxdddddocl0NNXXXKkkXWWWWWWNXXNWWNOdkKXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXKKKKKKKKXXXK0xl::::;:lxOOO0Okkk
KNNNNNNNNX0xlc::;;:;;d0XKKKXXNNNK0OO00XXXNNNNNNXXXXXXXXNNNNNNNNNNNNXXNN0dONWWWWWWMMMMMMMMMMMMWWWWWWMMN0xdddddo:o0K0KXXOx0WWWWWWWN00NWWWNOdkXNXK00KXXXXXXXXXXXKKXXXXXKKKKKKKKKKKKKKKKKKOdc:::;::oOOO00OOO
KNNNNNNNXkc,;::;;;,:dKNNNXXXNNXXXXNNNNNNNNNNNNNXXNNNNXNNNNNNNXXNNNXXXNXkkXWWWWWWWMMMMMMMMMMMWWWWWWWWWXkddddddl:dKK000kdkXWWWWWWWX0KWWWWWNkodOOkddOXXXKKXKKKXXXKKKXXXKKKXKKKXNNXXXKK0KKKkl:::::cokOOO0000
0NNNNNXKOo:;::,,:;l0XNNNNNXXKkxOXXXNXNNNNNXXNX00KXNNNNNNNNXXXXXXXXNXNNOx0WWWWWWWMMMMMMMMMMWWWWWWWWWWWKxddddddc;lkOkxo:cOWWWWWWWNKOXWWWWWWKo,;cclldk0KKKKXKXXXXXXXXXXXXXXOxk0XNXXXKKKKK0klc:::ccdO0OO0000
0K0KKOoc:cc:;'';;:xXNNNNNNNXKkxOXXXXXXXXNXKKXXK0KXK0kOKXNNXXXXXXXXNNNKkkXWWWWWWMMMMMMMMMWMWWWWWWWWWNX0dddddddc,:oddoc:oKWWWWWWWWXXNWWWWWWW0c',;;;;;:llloxO0000KXXXXXXXXXK0O0KXXKK000000xlccc:::ok00OOO00
00k00d:,:c:;,',;;:x0XNNNNNXXNNXXKkxOK0O0KKKXXK0OO0OxdkKXNXXNNXNNNNNNXOx0WMWWWWWWWMMWWWMWWWWWWWWWWWWXKkdddddddc,:clllcckNWWWWWWWWWWWWWWWWWWWKdc;,;;:;;;;;:::clk0KXXXXXXXXXXKKKKKKKK0000Ooccc::;cdkO000O00
0NKOdlc:;;;:;..;oOKXXK0KNNXNNXXNKOxdxxxxxdxkkdodk0KXXXXXXXXNNNNNNXX0OdkXWWWWWWWMMMMMWMMWWWWWWWWWWWWKOkddddddo:,:ccllcoKWWWWWWWWWWWWWWWWWWWWWWXOo:,;;;;;;;;:ldOKXXKKKXXXXXXKKKKKKKKKKK0xc:::::;cdOOO00OO0
00o;;::;;:;;,,ckKKKXXK0KNNX0O0XNXKxlcloooooooodkOOkk0KKOO0XX0OOKXXKOxoONWWWWWWMWWWWWWWWWWWWWWWWWWWN0kxddddddo:,:c::::xNWWWWWWMWWWWWWWWNNNWWWWWWNk:,;;;;:ldk0KKXXKKKKXXXXXXXKXXKKKKKKOdc:;::::;;codxO0OO0
00o;',;;::;:d0XXXXXXNNNNNNXOxOXNNX0dlcccclolllodddddddxkkkOkooxOOOKKxdKWWWWWWWMMWWWWWWWWWWWWWWWWWWN0xdddddddo:':c:;;cOWWWWWWWWWWWWWWWKxloOXNWWN0o;;:loxk0KKKKKKKKKKKXXXXXKKXXXKKKKKOoc::::::c:;:cldO0OOO
c:;,;:;',;:dKNNNNNNNNNNNNNXXKKXXK0KK0kdlcccccclccloooooolloolddlld0OdkNWWWWWWWWWWWWWWWWWWWWWWWWWWWXOddddddddo;':c:;,lKWWNXXNWWWWWWMW0l,,,;coxxxoodkOKXXXXXXXXXXKKKXXXXXXXXKKKKXXXXKOoc:::c:clc;;::oO00OO
';:cc;;;;cxKNNNNNNNNNNNNNNXXXXN0kkOKKKXKOxdollc:;;;::cccc::ccc:;;colcONWWWWWWWWWWWWWWWWWWWWWWWWWWWXkdoodooddo;';:;,,oXWN0xxOXXNWWWWNx;:lodxkO0KXXXK0KXXXXXXXKKKKOkKXXXXXXXKKKKXXXXX0d:;;:::clc:;;;o0K00O
,;',;ckK0KXXXKKXXXXKKKXXNNNNXXXXXNXOk0XNNNNXK0Okdollcccc:;:ccc;;;::;cOWWWWWWNWWWWWWWWWWWWWWWWWWWWWNOxdooooodo;.'::;;xXWWXOkOK00XWWWKdd0KXXNNXXXNNNXXXXXKKXXXXXXXK0KNNXXXXXXXXXXXXXXXkc;;;;;:cc:;,:d0000O
''',o0NNXK0KKOxxxxxdddxkkOKXKKKXNNNXXXXXXNNNNNNNNXXXK00OOkxkkkxdool:cxOOOOOOOOOOOOOOOOOO000OOOOOOOkxdoolloooollxO0OdkXWWWXOONXOKWMWKdkXNXXNNXXNNNXXXXNXKKXNNNXXNNNNXXKK0000OkkkkkkkOkl;;;;;:c:::;ckK0O0O
coox0XNN0xdxkkkOxoloooodxOKXXNXXNNNNNXXNNNNNNNNNNNNNNNNNNXXXNNXXXXXK000KKKKKXXXXKKKKKK00000000KKKKKKKKKKKXXXXXXXXXXOx0WWWW0kKXOONWW0dOXNNXXNNNNNXXNNXNNNXXNNNNNNNXXXKKKK0Okxdoollllooc:;;;::::::;lOX0O0O
x0KXXNNN0dlldOKXKOxxkkOKXXXNNNNNNNNNNNNNNNNNNNNNNXXXNNNNXXXXXNNNNNNNNNNNNNNNNNXKKXNNNNNNNNNNNWWWNNWXKOOKNNNXXNNNNNNKkkKWWW0kKXOONWNkx0XXXXXNNXXXXXXNNNNNNNXNNXNNNNNNXXXXXK0Okxddollllc:;,;;:c:::;l0XK00O
0NNNXXNNNK00KXXXXNNNXXNNNXXXNNNNNNNNNNNNNNNNNNNNXXXXXXNNNNNXXNNNNXXKXKKKKKKK0Okdooxk0XNWWWWWWWWWNXKKKKKKXXXXXXXXNXNX0kxO00k0NXOONWKxkXXXXXXXXXNNXXXXNNXXNNXXXXXNNNNNNNNXXXXXKKKK0Okxxdl:;;;:c::::oKNK00O
OXXXXNNNNNNNNNNXNNNNNXXNX0k0NNNNNNNNNNNNNNNNNNNNNNNXXK00OOkkO0K0OOkkkkkOOOOOOOkkxdlloxOKNNNNNNNNK0OO0KNWNXXXNXXXXXXXXXOodOKNWKOKWXkx0NNNNNNNXXNNXXXXXNNXNXXXNXXNNNNNXXXXXNNXXXNNNNXXK0kkkxollllllxKXK0OO
0XXXNNNNNXXXXXNNNNNXXXXNNXXXNNNNNNNNNNNNNNNNNNNWWWWWNNX0OkdoodkOkkkxkkkOOOOOOOOOkkxdolld0XNWWWWWWWWWWWWMWWXXNNXXNXXNNX0x0NWWXOONXkdONNXXXXNNNNNNXXXXXXXNNNNNNNXNNNNNNXXNNNNNNNXNNXXXXKKKXXXKKKKK00KK0000
0NNNNNNNXXK0OO00000000KXNNNNNNNNNNNNNNNNNNNNNNNXXNWNNWWNNKOxoooxkOkxxxkOOOOOOOOOkkOOkdlldKNWWWWWWWWNNNXXXNNNNNXNNXXXXXOdONWXkdkOxx0NNNXXXXNNNNNNNNNXXXXNNNNNNXXNNNNNNXXXNNNNNNNNNXXXXXXXXXXXXXXKKKKKKK00
0NNXXXNXXXKOdooddddxOKXXXXNNNNNNXXXNNNWWNNXXK000XNNNWWWWWNXKOdooxOOOkkOOOOOOOOOOOOOOOkdlokXNXKKKK00OOOxdxOKNXXXNNNXXXX0ddkkxdxkO0XNNNXXXXNNNNNNNXXXNNNNNNNNNXXNNNXXXNNNNNNNNNNXXXXXXXXXXXKKKKXXXKKKKKKKK
0NXXXXXXXNXKkolloxOKKXNNNNNNNXNNXXNNWWNXKKK0O0KNNWWWWWWWWWWNKkdook0OOkkOOOOOOkkkOkkkkxdlldO0Okkkkkkkkkxoox0XXXXXXXXNNNXK0O0KXNNNNNNNNNNXXXNNNNNXK0XNNNNNNNXXXXNXKKXXNNNNNNNNNNXK00KKKKKKKXXKXXXXKKKKKXXX
0NXNXXXXXXXXXK000KXXXXNNNNNNNXXXXNNWNNXK0Okkk0XNWWWWWWWWWWWNX0xoldkkkxxkxxxdddooolccc:cccdkkkkkkkxxkkkxoodOKKXNNXXNNNNNNNNNNNNNNNNNNNNNNXXXXXNXXXXNNNNNNNNNXXXNXXXXK0KXXXNNNNN0doxOOkOOO0KKKXXKKXXXKKKKK
0XXNXXXXXXNNXXXXXXXXXXXXXXXNXXXNWWWNXKKK0kxddkKXNNNXXNNNNXXXK0xolllllllcccccc:cc::ccccllodxxxkkkxxxxxxddodOKXXNNNNNNNNNNXXXXNNNNNNNXXNNNNNNNNNXXNNNNNNNNNNNXXXXXXXXXXXXXXXXXX0koodxkO0O0KKXXXXXXNXXKKKXX
k0000000000K0000000000000000KKKKKKK0OkkxdolllodkOOOOOOOOOOOOOkoc:::::::::;::cccccllllloooododdddddooooolloxO00000K0000K000000000000000000000000000K00000KKK0000OO000000000OOkdodxkO00OO000O0Okk0K0OOOOOO

 * 
 * 
 */