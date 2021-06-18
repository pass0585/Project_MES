#region < HEADER AREA >
// *---------------------------------------------------------------------------------------------*
//   Form ID      : WM_Inspection
//   Form Name    : 수입 검사 결과 등록
//   Name Space   : KFQS_Form
//   Created Date : 2021/06
//   Made By      : LJH
//   Description  : 
// *---------------------------------------------------------------------------------------------*
#endregion

#region < USING AREA >
using System;
using System.Data;
using DC_POPUP;
using DC00_assm;
using DC00_WinForm;

using Infragistics.Win.UltraWinGrid;
#endregion

namespace KFQS_Form
{
    public partial class WM_Inspection : DC00_WinForm.BaseMDIChildForm
    {

        #region < MEMBER AREA >
        DataTable rtnDtTemp        = new DataTable(); // 
        UltraGridUtil _GridUtil    = new UltraGridUtil();  //그리드 객체 생성
        Common _Common             = new Common();
        string plantCode           = LoginInfo.PlantCode;

        #endregion


        #region < CONSTRUCTOR >
        public WM_Inspection()
        {
            InitializeComponent();
        }
        #endregion


        #region < FORM EVENTS >
        private void WM_Inspection_Load(object sender, EventArgs e)
        {
            #region ▶ GRID ◀
            _GridUtil.InitializeGrid(this.grid1, true, true, false, "", false);
            _GridUtil.InitColumnUltraGrid(grid1, "PLANTCODE", "공장", true, GridColDataType_emu.VarChar, 120, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "ITEMCODE", "품목", true, GridColDataType_emu.VarChar, 140, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "ITEMNAME", "품목명", true, GridColDataType_emu.VarChar, 140, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "ITEMTYPE", "품목구분", true, GridColDataType_emu.VarChar, 120, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "LOTNO", "LOTNO", true, GridColDataType_emu.VarChar, 120, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "WHCODE", "입고창고", true, GridColDataType_emu.VarChar, 120, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "STOCKQTY", "재고수량", true, GridColDataType_emu.Double, 100, 120, Infragistics.Win.HAlign.Right, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "UNITCODE", "단위", true, GridColDataType_emu.VarChar, 100, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.SetInitUltraGridBind(grid1);

            _GridUtil.InitializeGrid(this.grid2, true, true, false, "", false);
            _GridUtil.InitColumnUltraGrid(grid2, "CHK", "합격여부", true, GridColDataType_emu.CheckBox, 80, 120, Infragistics.Win.HAlign.Left, true, true);
            _GridUtil.InitColumnUltraGrid(grid2, "PLANTCODE", "공장",    true, GridColDataType_emu.VarChar, 120, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid2, "ITEMCODE", "품목코드", true, GridColDataType_emu.VarChar, 120, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid2, "ITEMNAME",   "품목명", true, GridColDataType_emu.VarChar, 80, 120, Infragistics.Win.HAlign.Center, true, false);
            _GridUtil.InitColumnUltraGrid(grid2, "INSPCODE",     "검사코드",   true, GridColDataType_emu.VarChar, 120, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid2, "INSPDETAIL",  "검사내역",    true, GridColDataType_emu.VarChar, 120, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.SetInitUltraGridBind(grid2);
            #endregion

            #region ▶ COMBOBOX ◀

            #endregion

            #region ▶ POP-UP ◀
            #endregion

            #region ▶ ENTER-MOVE ◀

            #endregion
        }
        #endregion


        #region < TOOL BAR AREA >
        public override void DoInquire()
        {
            DoFind();
        }
        private void DoFind()
        {
            DBHelper helper = new DBHelper(false);
            try
            {
                base.DoInquire();
                _GridUtil.Grid_Clear(grid1);
                _GridUtil.Grid_Clear(grid2);
                string sLotNo = Convert.ToString(txtLotNo.Text);

                rtnDtTemp = helper.FillTable("18WM_Inspection_S1", CommandType.StoredProcedure
                                    , helper.CreateParameter("LOTNO",   sLotNo,  DbType.String, ParameterDirection.Input)
                                    );

                this.ClosePrgForm();
                this.grid1.DataSource = rtnDtTemp;
            }
            catch (Exception ex)
            {
                ShowDialog(ex.ToString(),DialogForm.DialogType.OK);    
            }
            finally
            {
                helper.Close();
            }
        }
        /// <summary>
        /// ToolBar의 신규 버튼 클릭
        /// </summary>
        public override void DoNew()
        {
            
        }
        /// <summary>
        /// ToolBar의 삭제 버튼 Click
        /// </summary>
        public override void DoDelete()
        {   
           
        }
        /// <summary>
        /// ToolBar의 저장 버튼 Click
        /// </summary>
        public override void DoSave()
        {
            DataTable dt = new DataTable();

            dt = grid2.chkChange();
            if (dt == null)
                return;
            DBHelper helper = new DBHelper("", false);

            try
            {
                //base.DoSave();

                if (this.ShowDialog("수입 검사 여부를 등록하시겠습니까?") == System.Windows.Forms.DialogResult.Cancel)
                {
                    return;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToString(dt.Rows[i]["CHK"]) == "0") continue;
                    //if (Convert.ToString(dt.Rows[i]["ITEMTYPE"]) != "ROH")
                    //{
                    //    ShowDialog("원자재가 아닌 LOT 는 원자재 출고 취소를 할 수 없습니다.", DialogForm.DialogType.OK);
                    //    helper.Rollback();
                    //    return;
                    //}

                    helper.ExecuteNoneQuery("WM_4_Inspection_I1"
                                            , CommandType.StoredProcedure
                                            , helper.CreateParameter("PLANTCODE", Convert.ToString(dt.Rows[i]["PLANTCODE"]), DbType.String, ParameterDirection.Input)
                                            , helper.CreateParameter("LOTNO", Convert.ToString(dt.Rows[i]["LOTNO"]), DbType.String, ParameterDirection.Input)
                                            , helper.CreateParameter("ITEMCODE", Convert.ToString(dt.Rows[i]["ITEMCODE"]), DbType.String, ParameterDirection.Input)
                                            , helper.CreateParameter("QTY", Convert.ToString(dt.Rows[i]["STOCKQTY"]), DbType.String, ParameterDirection.Input)
                                            , helper.CreateParameter("UNITCODE", Convert.ToString(dt.Rows[i]["UNITCODE"]), DbType.String, ParameterDirection.Input)
                                            , helper.CreateParameter("WORKERID", this.WorkerID, DbType.String, ParameterDirection.Input));

                    helper.ExecuteNoneQuery("18PP_STockPP_U1"
                                            , CommandType.StoredProcedure
                                            , helper.CreateParameter("PLANTCODE", Convert.ToString(dt.Rows[i]["PLANTCODE"]), DbType.String, ParameterDirection.Input)
                                            , helper.CreateParameter("LOTNO", Convert.ToString(dt.Rows[i]["LOTNO"]), DbType.String, ParameterDirection.Input)
                                            , helper.CreateParameter("ITEMCODE", Convert.ToString(dt.Rows[i]["ITEMCODE"]), DbType.String, ParameterDirection.Input)
                                            , helper.CreateParameter("QTY", Convert.ToString(dt.Rows[i]["STOCKQTY"]), DbType.String, ParameterDirection.Input)
                                            , helper.CreateParameter("UNITCODE", Convert.ToString(dt.Rows[i]["UNITCODE"]), DbType.String, ParameterDirection.Input)
                                            , helper.CreateParameter("WORKERID", this.WorkerID, DbType.String, ParameterDirection.Input));

                    if (helper.RSCODE == "E")
                    {
                        this.ShowDialog(helper.RSMSG, DialogForm.DialogType.OK);
                        helper.Rollback();
                        return;
                    }
                }

                helper.Commit();
                this.ShowDialog("데이터가 저장 되었습니다.", DialogForm.DialogType.OK);
                this.ClosePrgForm();
                DoInquire();
            }
            catch (Exception ex)
            {
                helper.Rollback();
                ShowDialog(ex.ToString());
            }
            finally
            {
                helper.Close();
            }
        }
        #endregion

        private void grid1_AfterRowActivate(object sender, EventArgs e)
        {
            _GridUtil.Grid_Clear(grid2);
            DBHelper helper = new DBHelper(false);
            try
            {
                string sLotNo     = Convert.ToString(grid1.ActiveRow.Cells["LOTNO"].Value);

                rtnDtTemp = helper.FillTable("18WM_Inspection_S3", CommandType.StoredProcedure
                                    , helper.CreateParameter("LOTNO", sLotNo, DbType.String, ParameterDirection.Input)
                                    );

                this.ClosePrgForm();
                this.grid2.DataSource = rtnDtTemp;
            }
            catch (Exception ex)
            {
                ShowDialog(ex.ToString(), DialogForm.DialogType.OK);
            }
            finally
            {
                helper.Close();
            }
        }
    }
}




