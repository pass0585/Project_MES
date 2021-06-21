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
            _GridUtil.InitColumnUltraGrid(grid1, "ITEMTYPE", "품목구분", true, GridColDataType_emu.VarChar, 120, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "ITEMCODE", "품목", true, GridColDataType_emu.VarChar, 140, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "ITEMNAME", "품목명", true, GridColDataType_emu.VarChar, 140, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "STOCKQTY", "품목수량", true, GridColDataType_emu.VarChar, 120, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "UNITCODE", "단위", true, GridColDataType_emu.VarChar, 100, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "LOTNO", "LOTNO", true, GridColDataType_emu.VarChar, 120, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid1, "WHCODE", "입고창고", true, GridColDataType_emu.VarChar, 120, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.SetInitUltraGridBind(grid1);

            _GridUtil.InitializeGrid(this.grid2, true, true, false, "", false);
            _GridUtil.InitColumnUltraGrid(grid2, "CHK", "합격여부", true, GridColDataType_emu.CheckBox, 80, 120, Infragistics.Win.HAlign.Left, true, true);
            _GridUtil.InitColumnUltraGrid(grid2, "PLANTCODE", "공장", true, GridColDataType_emu.VarChar, 120, 120, Infragistics.Win.HAlign.Left, false, false);
            _GridUtil.InitColumnUltraGrid(grid2, "ITEMCODE", "품목코드", true, GridColDataType_emu.VarChar, 120, 120, Infragistics.Win.HAlign.Left, false, false);
            _GridUtil.InitColumnUltraGrid(grid2, "ITEMNAME", "품목명", true, GridColDataType_emu.VarChar, 80, 120, Infragistics.Win.HAlign.Center, false, false);
            _GridUtil.InitColumnUltraGrid(grid2, "INSPCODE", "검사코드", true, GridColDataType_emu.VarChar, 120, 120, Infragistics.Win.HAlign.Left, true, false);
            _GridUtil.InitColumnUltraGrid(grid2, "INSPDETAIL", "검사내역", true, GridColDataType_emu.VarChar, 300, 120, Infragistics.Win.HAlign.Left, true, false);
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

                rtnDtTemp = helper.FillTable("WM_4_Inspection_S1", CommandType.StoredProcedure
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
            this.grid1.UpdateData();
            DataTable dt = (DataTable)grid2.DataSource;
                
            if (dt == null)
                return;


            DBHelper helper = new DBHelper("", true);
            try
            {
                if (this.ShowDialog("수입검사 내역을 저장하시겠습니까? ") == System.Windows.Forms.DialogResult.Cancel) return;
                string sInspNo = string.Empty;
               
                foreach (DataRow drRow in dt.Rows)
                {
                    
                     helper.ExecuteNoneQuery("WM_4_Inspection_I1", CommandType.StoredProcedure
                                           , helper.CreateParameter("PLANTCODE", Convert.ToString(drRow["PLANTCODE"]), DbType.String, ParameterDirection.Input)
                                           , helper.CreateParameter("ITEMCODE", Convert.ToString(drRow["ITEMCODE"]), DbType.String, ParameterDirection.Input)
                                           , helper.CreateParameter("WHCODE", Convert.ToString(this.grid1.ActiveRow.Cells["WHCODE"].Value), DbType.String, ParameterDirection.Input)
                                           , helper.CreateParameter("LOTNO", Convert.ToString(this.grid1.ActiveRow.Cells["LOTNO"].Value), DbType.String, ParameterDirection.Input)
                                           , helper.CreateParameter("MAKER", LoginInfo.UserID, DbType.String, ParameterDirection.Input)
                                           , helper.CreateParameter("INSPDETAIL", Convert.ToString(drRow["INSPDETAIL"]), DbType.String, ParameterDirection.Input)
                                           , helper.CreateParameter("INSPRESULT_B", Convert.ToString(drRow["CHK"]), DbType.String, ParameterDirection.Input)
                                           , helper.CreateParameter("STOCKQTY", Convert.ToString(this.grid1.ActiveRow.Cells["STOCKQTY"].Value), DbType.String, ParameterDirection.Input)
                                           , helper.CreateParameter("INSPNO", sInspNo, DbType.String, ParameterDirection.Input)
                                           );
                     if (helper.RSCODE == "S")
                     {
                         sInspNo = helper.RSMSG;
                     }
                     else break;
                     #endregion
                    
                    if (helper.RSCODE != "S") break;
                }
                if (helper.RSCODE != "S")
                {
                    this.ClosePrgForm();
                    helper.Rollback();
                    this.ShowDialog(helper.RSMSG, DialogForm.DialogType.OK);
                    return;
                }
                helper.Commit();
                this.ClosePrgForm();
                this.ShowDialog("데이터가 저장 되었습니다.", DialogForm.DialogType.OK);
                DoInquire();
            }
            catch (Exception ex)
            {
                CancelProcess = true;
                helper.Rollback();
                ShowDialog(ex.ToString());
            }
            finally
            {
                helper.Close();
            }
        }

        private void grid1_AfterRowActivate(object sender, EventArgs e)
        {
            _GridUtil.Grid_Clear(grid2);
            DBHelper helper = new DBHelper(false);
            try
            {
                string sLotNo     = Convert.ToString(this.grid1.ActiveRow.Cells["LOTNO"].Value);

                rtnDtTemp = helper.FillTable("WM_4_Inspection_S2", CommandType.StoredProcedure
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




